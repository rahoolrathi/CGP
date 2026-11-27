using CgbSteps.Api.Data;
using CgbSteps.Api.Helpers;
using CgbSteps.Api.Models.Dtos.Common;
using CgbSteps.Api.Models.Dtos.Employees;
using CgbSteps.Api.Models.Entities;
using CgbSteps.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CgbSteps.Api.Repositories;

public sealed class EmployeeRepository(AppDbContext db) : IEmployeeRepository
{
    public async Task<PagedResponse<EmployeeResponse>> ListAsync(
        EmployeeListQuery query,
        CancellationToken cancellationToken)
    {
        var employees = db.Employees.AsNoTracking();

        if (query.DepartmentId is { } departmentId)
        {
            employees = employees.Where(e => e.DepartmentId == departmentId);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{LikeHelper.Escape(query.Search.Trim())}%";
            employees = employees.Where(e =>
                EF.Functions.ILike(e.FullName, pattern, LikeHelper.EscapeCharacter)
                || EF.Functions.ILike(e.EmployeeId, pattern, LikeHelper.EscapeCharacter)
                || EF.Functions.ILike(e.WorkEmail, pattern, LikeHelper.EscapeCharacter));
        }

        var total = await employees.CountAsync(cancellationToken);
        var items = await employees
            .OrderBy(e => e.FullName)
            .ThenBy(e => e.Id)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(e => new EmployeeResponse(
                e.Id, e.EmployeeId, e.FullName, e.WorkEmail, e.DepartmentId, e.Department.Name, e.CreatedAt))
            .ToListAsync(cancellationToken);

        return new PagedResponse<EmployeeResponse>(items, query.Page, query.PageSize, total);
    }

    public Task<EmployeeResponse?> GetResponseByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Employees
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EmployeeResponse(
                e.Id, e.EmployeeId, e.FullName, e.WorkEmail, e.DepartmentId, e.Department.Name, e.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Employees.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public Task<bool> EmployeeIdExistsAsync(string employeeId, CancellationToken cancellationToken) =>
        db.Employees.AnyAsync(e => e.EmployeeId == employeeId, cancellationToken);

    public Task<bool> WorkEmailExistsAsync(string workEmail, CancellationToken cancellationToken) =>
        db.Employees.AnyAsync(e => e.WorkEmail == workEmail, cancellationToken);

    public async Task<(HashSet<string> EmployeeIds, HashSet<string> WorkEmails)> FindExistingAsync(
        IReadOnlyCollection<string> employeeIds,
        IReadOnlyCollection<string> workEmails,
        CancellationToken cancellationToken)
    {
        var takenIds = await db.Employees
            .Where(e => employeeIds.Contains(e.EmployeeId))
            .Select(e => e.EmployeeId)
            .ToListAsync(cancellationToken);

        var takenEmails = await db.Employees
            .Where(e => workEmails.Contains(e.WorkEmail))
            .Select(e => e.WorkEmail)
            .ToListAsync(cancellationToken);

        return ([.. takenIds], [.. takenEmails]);
    }

    public void Add(Employee employee) => db.Employees.Add(employee);

    public void Remove(Employee employee) => db.Employees.Remove(employee);

    public Task SaveChangesAsync(CancellationToken cancellationToken) => db.SaveChangesAsync(cancellationToken);
}
