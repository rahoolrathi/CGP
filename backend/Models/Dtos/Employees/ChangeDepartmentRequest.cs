using System.ComponentModel.DataAnnotations;

namespace CgbSteps.Api.Models.Dtos.Employees;

public sealed class ChangeDepartmentRequest
{
    [Required]
    public Guid? DepartmentId { get; init; }
}
