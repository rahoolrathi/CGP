using CgbSteps.Api.Exceptions;
using CgbSteps.Api.Helpers;
using CgbSteps.Api.Models.Dtos.Common;
using CgbSteps.Api.Models.Dtos.Employees;
using CgbSteps.Api.Models.Entities;
using CgbSteps.Api.Repositories.Interfaces;
using CgbSteps.Api.Services.Interfaces;

namespace CgbSteps.Api.Services;

public sealed class EmployeeService(
    IEmployeeRepository employees,
    IDepartmentRepository departments,
    TimeProvider clock) : IEmployeeService
{
    public Task<PagedResponse<EmployeeResponse>> ListAsync(
        EmployeeListQuery query,
        CancellationToken cancellationToken) =>
        employees.ListAsync(query, cancellationToken);

    public async Task<EmployeeResponse> CreateAsync(
        CreateEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var employeeId = EmployeeRules.NormalizeEmployeeId(request.EmployeeId);
        var workEmail = EmployeeRules.NormalizeEmail(request.WorkEmail);
        var departmentId = request.DepartmentId!.Value;

        if (await departments.GetByIdAsync(departmentId, cancellationToken) is null)
        {
            throw AppException.BadRequest(AppConstants.ErrorCodes.DepartmentNotFound);
        }

        if (await employees.EmployeeIdExistsAsync(employeeId, cancellationToken))
        {
            throw AppException.Conflict(AppConstants.ErrorCodes.EmployeeIdExists);
        }

        if (await employees.WorkEmailExistsAsync(workEmail, cancellationToken))
        {
            throw AppException.Conflict(AppConstants.ErrorCodes.WorkEmailExists);
        }

        var employee = Employee.Create(
            employeeId, request.FullName.Trim(), workEmail, departmentId, clock.GetUtcNow());
        employees.Add(employee);
        await employees.SaveChangesAsync(cancellationToken);

        return await GetResponseAsync(employee.Id, cancellationToken);
    }

    public async Task<EmployeeResponse> ChangeDepartmentAsync(
        Guid id,
        ChangeDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var departmentId = request.DepartmentId!.Value;

        var employee = await employees.GetByIdAsync(id, cancellationToken)
            ?? throw AppException.NotFound(AppConstants.ErrorCodes.EmployeeNotFound);

        if (await departments.GetByIdAsync(departmentId, cancellationToken) is null)
        {
            throw AppException.BadRequest(AppConstants.ErrorCodes.DepartmentNotFound);
        }

        employee.ChangeDepartment(departmentId);
        await employees.SaveChangesAsync(cancellationToken);

        return await GetResponseAsync(id, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var employee = await employees.GetByIdAsync(id, cancellationToken)
            ?? throw AppException.NotFound(AppConstants.ErrorCodes.EmployeeNotFound);

        employees.Remove(employee);
        await employees.SaveChangesAsync(cancellationToken);
    }

    private async Task<EmployeeResponse> GetResponseAsync(Guid id, CancellationToken cancellationToken) =>
        await employees.GetResponseByIdAsync(id, cancellationToken)
        ?? throw AppException.NotFound(AppConstants.ErrorCodes.EmployeeNotFound);
}
