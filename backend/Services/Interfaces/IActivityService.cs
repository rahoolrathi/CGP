using CgbSteps.Api.Models.Dtos.Activity;

namespace CgbSteps.Api.Services.Interfaces;

public interface IActivityService
{
    /// <summary>Saves daily totals for an employee. Re-sending a day replaces it.</summary>
    Task<IReadOnlyList<ActivityDayResponse>> SyncAsync(
        Guid employeeId,
        SyncActivityRequest request,
        CancellationToken cancellationToken);

    /// <summary>Returns the employee's days in the range, oldest first. Defaults to the last 30 days.</summary>
    Task<IReadOnlyList<ActivityDayResponse>> ListAsync(
        Guid employeeId,
        DateOnly? startDate,
        DateOnly? endDate,
        CancellationToken cancellationToken);
}
