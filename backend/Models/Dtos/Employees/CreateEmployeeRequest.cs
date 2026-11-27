using System.ComponentModel.DataAnnotations;
using CgbSteps.Api.Helpers;

namespace CgbSteps.Api.Models.Dtos.Employees;

public sealed class CreateEmployeeRequest
{
    [Required]
    [RegularExpression(EmployeeRules.EmployeeIdPattern)]
    public string EmployeeId { get; init; } = string.Empty;

    [Required]
    [StringLength(EmployeeRules.FullNameMaxLength, MinimumLength = EmployeeRules.FullNameMinLength)]
    public string FullName { get; init; } = string.Empty;

    [Required]
    [StringLength(EmployeeRules.WorkEmailMaxLength)]
    [EmailAddress]
    public string WorkEmail { get; init; } = string.Empty;

    [Required]
    public Guid? DepartmentId { get; init; }
}
