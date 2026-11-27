using CgbSteps.Api.Models.Dtos.Activity;
using CgbSteps.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CgbSteps.Api.Controllers;

[ApiController]
[Route("api/v1/employees/{employeeId:guid}/activity")]
[AllowAnonymous] // TODO: only the signed-in employee (or an admin) once login is added.
public sealed class ActivityController(IActivityService activityService) : ControllerBase
{
    /// <summary>Daily steps, distance, active minutes and calories. Defaults to the last 30 days.</summary>
    [HttpGet]
    public Task<IReadOnlyList<ActivityDayResponse>> List(
        Guid employeeId,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken cancellationToken) =>
        activityService.ListAsync(employeeId, from, to, cancellationToken);

    /// <summary>Save daily totals from Health Connect. Sending a day again replaces it.</summary>
    [HttpPut]
    public Task<IReadOnlyList<ActivityDayResponse>> Sync(
        Guid employeeId,
        SyncActivityRequest request,
        CancellationToken cancellationToken) =>
        activityService.SyncAsync(employeeId, request, cancellationToken);
}
