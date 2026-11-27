using CgbSteps.Api.Exceptions;
using CgbSteps.Api.Helpers;
using CgbSteps.Api.Models.Dtos.Employees;
using CgbSteps.Api.Models.Entities;
using CgbSteps.Api.Repositories.Interfaces;
using CgbSteps.Api.Services.Interfaces;

namespace CgbSteps.Api.Services;

public sealed class EmployeeImportService(
    IEmployeeRepository employees,
    IDepartmentRepository departments,
    TimeProvider clock) : IEmployeeImportService
{
    public const long MaxFileBytes = 2 * 1024 * 1024;
    private const int MaxRows = 10_000;
    private static readonly string[] Columns = ["employeeId", "fullName", "workEmail", "department"];

    public async Task<ImportEmployeesResponse> ImportAsync(IFormFile? file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            throw AppException.BadRequest(AppConstants.ErrorCodes.FileMissing);
        }

        if (file.Length > MaxFileBytes)
        {
            throw AppException.BadRequest(AppConstants.ErrorCodes.FileTooLarge);
        }

        await using var stream = file.OpenReadStream();
        var lines = await CsvHelper.ReadAllAsync(stream, cancellationToken);
        var columnIndex = ReadHeader(lines);

        if (lines.Count - 1 > MaxRows)
        {
            throw AppException.BadRequest(AppConstants.ErrorCodes.CsvTooManyRows);
        }

        var errors = new List<ImportRowError>();
        var rows = ParseRows(lines, columnIndex, errors);

        var (takenIds, takenEmails) = await employees.FindExistingAsync(
            rows.Select(r => r.EmployeeId).ToList(),
            rows.Select(r => r.WorkEmail).ToList(),
            cancellationToken);

        var departmentsByName = new Dictionary<string, Department>(StringComparer.OrdinalIgnoreCase);
        foreach (var existing in await departments.GetAllAsync(cancellationToken))
        {
            departmentsByName.TryAdd(existing.Name, existing);
        }

        var now = clock.GetUtcNow();
        int added = 0, skipped = 0, departmentsCreated = 0;

        foreach (var row in rows)
        {
            if (takenIds.Contains(row.EmployeeId) || takenEmails.Contains(row.WorkEmail))
            {
                skipped++;
                continue;
            }

            if (!departmentsByName.TryGetValue(row.Department, out var department))
            {
                department = Department.Create(row.Department, now);
                departments.Add(department);
                departmentsByName[row.Department] = department;
                departmentsCreated++;
            }

            employees.Add(Employee.Create(row.EmployeeId, row.FullName, row.WorkEmail, department.Id, now));
            added++;
        }

        await employees.SaveChangesAsync(cancellationToken);

        return new ImportEmployeesResponse(added, skipped, departmentsCreated, errors);
    }

    private static Dictionary<string, int> ReadHeader(List<string[]> lines)
    {
        var header = lines.Count > 0 ? lines[0] : [];
        var index = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < header.Length; i++)
        {
            index.TryAdd(header[i].TrimStart('﻿'), i);
        }

        if (!Columns.All(index.ContainsKey))
        {
            throw AppException.BadRequest(AppConstants.ErrorCodes.CsvHeaderInvalid);
        }

        return index;
    }

    private static List<RosterRow> ParseRows(
        List<string[]> lines,
        Dictionary<string, int> column,
        List<ImportRowError> errors)
    {
        var rows = new List<RosterRow>();
        var seenIds = new HashSet<string>(StringComparer.Ordinal);
        var seenEmails = new HashSet<string>(StringComparer.Ordinal);

        for (var i = 1; i < lines.Count; i++)
        {
            var fields = lines[i];
            var rowNumber = i + 1;

            if (fields.All(string.IsNullOrWhiteSpace))
            {
                continue;
            }

            string Field(string name) => column[name] < fields.Length ? fields[column[name]] : string.Empty;

            var employeeId = EmployeeRules.NormalizeEmployeeId(Field("employeeId"));
            var fullName = Field("fullName").Trim();
            var workEmail = EmployeeRules.NormalizeEmail(Field("workEmail"));
            var department = Field("department").Trim();

            var error = Validate(employeeId, fullName, workEmail, department)
                ?? (seenIds.Add(employeeId) && seenEmails.Add(workEmail) ? null : AppConstants.ErrorCodes.DuplicateInFile);

            if (error is not null)
            {
                errors.Add(new ImportRowError(rowNumber, error));
                continue;
            }

            rows.Add(new RosterRow(employeeId, fullName, workEmail, department));
        }

        return rows;
    }

    private static string? Validate(string employeeId, string fullName, string workEmail, string department)
    {
        if (!EmployeeRules.IsValidEmployeeId(employeeId))
        {
            return AppConstants.ErrorCodes.InvalidEmployeeId;
        }

        if (!EmployeeRules.IsValidFullName(fullName))
        {
            return AppConstants.ErrorCodes.InvalidFullName;
        }

        if (!EmployeeRules.IsValidEmail(workEmail))
        {
            return AppConstants.ErrorCodes.InvalidWorkEmail;
        }

        return EmployeeRules.IsValidDepartmentName(department) ? null : AppConstants.ErrorCodes.InvalidDepartment;
    }

    private sealed record RosterRow(string EmployeeId, string FullName, string WorkEmail, string Department);
}
