using CgbSteps.Api.Models.Dtos.Common;
using CgbSteps.Api.Models.Dtos.Employees;
using CgbSteps.Api.Services;
using CgbSteps.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CgbSteps.Api.Controllers;

[ApiController]
[Route("api/v1/employees")]
[AllowAnonymous] // TODO: restrict to admins once admin login is added.
public sealed class EmployeesController(
    IEmployeeService employeeService,
    IEmployeeImportService importService) : ControllerBase
{
    [HttpGet]
    public Task<PagedResponse<EmployeeResponse>> List(
        [FromQuery] EmployeeListQuery query,
        CancellationToken cancellationToken) =>
        employeeService.ListAsync(query, cancellationToken);

    [HttpPost]
    [ProducesResponseType<EmployeeResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var employee = await employeeService.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, employee);
    }

    [HttpPatch("{id:guid}/department")]
    public Task<EmployeeResponse> ChangeDepartment(
        Guid id,
        ChangeDepartmentRequest request,
        CancellationToken cancellationToken) =>
        employeeService.ChangeDepartmentAsync(id, request, cancellationToken);

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await employeeService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>Upload a CSV with the header employeeId,fullName,workEmail,department.</summary>
    [HttpPost("import")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(EmployeeImportService.MaxFileBytes + (64 * 1024))]
    public Task<ImportEmployeesResponse> Import(IFormFile? file, CancellationToken cancellationToken) =>
        importService.ImportAsync(file, cancellationToken);
}
