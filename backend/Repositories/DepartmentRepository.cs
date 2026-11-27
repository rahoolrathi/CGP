using CgbSteps.Api.Data;
using CgbSteps.Api.Helpers;
using CgbSteps.Api.Models.Dtos.Departments;
using CgbSteps.Api.Models.Entities;
using CgbSteps.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CgbSteps.Api.Repositories;

public sealed class DepartmentRepository(AppDbContext db) : IDepartmentRepository
{
    public async Task<IReadOnlyList<DepartmentResponse>> ListAsync(CancellationToken cancellationToken) =>
        await db.Departments
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .Select(d => new DepartmentResponse(d.Id, d.Name, d.Employees.Count, d.CreatedAt))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken cancellationToken) =>
        await db.Departments.ToListAsync(cancellationToken);

    public Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Departments.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    public Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken) =>
        db.Departments.AnyAsync(
            d => EF.Functions.ILike(d.Name, LikeHelper.Escape(name), LikeHelper.EscapeCharacter),
            cancellationToken);

    public Task<bool> HasEmployeesAsync(Guid id, CancellationToken cancellationToken) =>
        db.Employees.AnyAsync(e => e.DepartmentId == id, cancellationToken);

    public void Add(Department department) => db.Departments.Add(department);

    public void Remove(Department department) => db.Departments.Remove(department);

    public Task SaveChangesAsync(CancellationToken cancellationToken) => db.SaveChangesAsync(cancellationToken);
}
