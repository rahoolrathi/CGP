using CgbSteps.Api.Models.Dtos.Departments;
using CgbSteps.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CgbSteps.Api.Controllers;

[ApiController]
[Route("api/v1/departments")]
[AllowAnonymous] // TODO: restrict to admins once admin login is added.
public sealed class DepartmentsController(IDepartmentService departmentService) : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<DepartmentResponse>> List(CancellationToken cancellationToken) =>
        departmentService.ListAsync(cancellationToken);

    [HttpPost]
    [ProducesResponseType<DepartmentResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var department = await departmentService.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, department);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await departmentService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
