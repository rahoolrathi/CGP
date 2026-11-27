using System.ComponentModel.DataAnnotations;

namespace CgbSteps.Api.Models.Dtos.Employees;

public sealed class EmployeeListQuery
{
    /// <summary>Matches part of the name, employee ID or work email.</summary>
    [StringLength(100)]
    public string? Search { get; init; }

    public Guid? DepartmentId { get; init; }

    [Range(1, 100_000)]
    public int Page { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 20;
}
