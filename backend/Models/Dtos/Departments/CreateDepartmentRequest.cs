using System.ComponentModel.DataAnnotations;

namespace CgbSteps.Api.Models.Dtos.Departments;

public sealed class CreateDepartmentRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; init; } = string.Empty;
}
