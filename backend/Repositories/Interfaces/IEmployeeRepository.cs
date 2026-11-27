using CgbSteps.Api.Models.Dtos.Common;
using CgbSteps.Api.Models.Dtos.Employees;
using CgbSteps.Api.Models.Entities;

namespace CgbSteps.Api.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<PagedResponse<EmployeeResponse>> ListAsync(EmployeeListQuery query, CancellationToken cancellationToken);

    Task<EmployeeResponse?> GetResponseByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> EmployeeIdExistsAsync(string employeeId, CancellationToken cancellationToken);

    Task<bool> WorkEmailExistsAsync(string workEmail, CancellationToken cancellationToken);

    /// <summary>Returns which of the given employee IDs and emails are already taken.</summary>
    Task<(HashSet<string> EmployeeIds, HashSet<string> WorkEmails)> FindExistingAsync(
        IReadOnlyCollection<string> employeeIds,
        IReadOnlyCollection<string> workEmails,
        CancellationToken cancellationToken);

    void Add(Employee employee);

    void Remove(Employee employee);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
