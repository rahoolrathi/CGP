namespace CgbSteps.Api.Models.Dtos.Employees;

/// <summary>Result of a roster import. Employees that already exist are skipped, not changed.</summary>
public sealed record ImportEmployeesResponse(
    int Added,
    int Skipped,
    int DepartmentsCreated,
    IReadOnlyList<ImportRowError> Errors);
