using CgbSteps.Api.Models.Dtos.Departments;
using CgbSteps.Api.Models.Entities;

namespace CgbSteps.Api.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<IReadOnlyList<DepartmentResponse>> ListAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken cancellationToken);

    Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken);

    Task<bool> HasEmployeesAsync(Guid id, CancellationToken cancellationToken);

    void Add(Department department);

    void Remove(Department department);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
