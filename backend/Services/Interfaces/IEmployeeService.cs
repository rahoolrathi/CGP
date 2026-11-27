using CgbSteps.Api.Models.Dtos.Common;
using CgbSteps.Api.Models.Dtos.Employees;

namespace CgbSteps.Api.Services.Interfaces;

public interface IEmployeeService
{
    Task<PagedResponse<EmployeeResponse>> ListAsync(EmployeeListQuery query, CancellationToken cancellationToken);

    Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken);

    Task<EmployeeResponse> ChangeDepartmentAsync(
        Guid id,
        ChangeDepartmentRequest request,
        CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
