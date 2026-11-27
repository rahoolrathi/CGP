using CgbSteps.Api.Models.Dtos.Departments;

namespace CgbSteps.Api.Services.Interfaces;

public interface IDepartmentService
{
    Task<IReadOnlyList<DepartmentResponse>> ListAsync(CancellationToken cancellationToken);

    Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
