namespace CgbSteps.Api.Models.Dtos.Employees;

public sealed record EmployeeResponse(
    Guid Id,
    string EmployeeId,
    string FullName,
    string WorkEmail,
    Guid DepartmentId,
    string DepartmentName,
    DateTimeOffset CreatedAt);
