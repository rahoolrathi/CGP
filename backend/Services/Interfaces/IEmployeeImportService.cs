using CgbSteps.Api.Models.Dtos.Employees;

namespace CgbSteps.Api.Services.Interfaces;

public interface IEmployeeImportService
{
    /// <summary>
    /// Imports a CSV roster with the header employeeId,fullName,workEmail,department.
    /// New employees are added, existing ones are skipped, and missing departments are created.
    /// </summary>
    Task<ImportEmployeesResponse> ImportAsync(IFormFile? file, CancellationToken cancellationToken);
}
