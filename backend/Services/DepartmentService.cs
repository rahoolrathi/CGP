using CgbSteps.Api.Exceptions;
using CgbSteps.Api.Helpers;
using CgbSteps.Api.Models.Dtos.Departments;
using CgbSteps.Api.Models.Entities;
using CgbSteps.Api.Repositories.Interfaces;
using CgbSteps.Api.Services.Interfaces;

namespace CgbSteps.Api.Services;

public sealed class DepartmentService(IDepartmentRepository departments, TimeProvider clock) : IDepartmentService
{
    public Task<IReadOnlyList<DepartmentResponse>> ListAsync(CancellationToken cancellationToken) =>
        departments.ListAsync(cancellationToken);

    public async Task<DepartmentResponse> CreateAsync(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();

        if (await departments.NameExistsAsync(name, cancellationToken))
        {
            throw AppException.Conflict(AppConstants.ErrorCodes.DepartmentExists);
        }

        var department = Department.Create(name, clock.GetUtcNow());
        departments.Add(department);
        await departments.SaveChangesAsync(cancellationToken);

        return new DepartmentResponse(department.Id, department.Name, 0, department.CreatedAt);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var department = await departments.GetByIdAsync(id, cancellationToken)
            ?? throw AppException.NotFound(AppConstants.ErrorCodes.DepartmentNotFound);

        if (await departments.HasEmployeesAsync(id, cancellationToken))
        {
            throw AppException.Conflict(AppConstants.ErrorCodes.DepartmentHasEmployees);
        }

        departments.Remove(department);
        await departments.SaveChangesAsync(cancellationToken);
    }
}
