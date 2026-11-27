namespace CgbSteps.Api.Models.Dtos.Departments;

public sealed record DepartmentResponse(Guid Id, string Name, int EmployeeCount, DateTimeOffset CreatedAt);
