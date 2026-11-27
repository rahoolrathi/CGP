using CgbSteps.Api.Exceptions;
using CgbSteps.Api.Helpers;
using CgbSteps.Api.Models.Dtos.Activity;
using CgbSteps.Api.Models.Entities;
using CgbSteps.Api.Repositories.Interfaces;
using CgbSteps.Api.Services.Interfaces;

namespace CgbSteps.Api.Services;

public sealed class ActivityService(
    IActivityRepository activities,
    IEmployeeRepository employees,
    TimeProvider clock) : IActivityService
{
    private const int MaxDaysBack = 30;
    private const int MaxRangeDays = 366;

    /// <summary>Days above this are saved but flagged for an admin to review.</summary>
    private const int FlagStepsAbove = 50_000;

    public async Task<IReadOnlyList<ActivityDayResponse>> SyncAsync(
        Guid employeeId,
        SyncActivityRequest request,
        CancellationToken cancellationToken)
    {
        await EnsureEmployeeExistsAsync(employeeId, cancellationToken);

        var now = clock.GetUtcNow();
        var today = QatarTimeHelper.ToQatarDate(now);
        var oldest = today.AddDays(-MaxDaysBack);

        // If a day is sent twice in one request, the last one wins.
        var days = request.Days
            .GroupBy(d => d.Date!.Value)
            .Select(g => g.Last())
            .ToList();

        if (days.Any(d => d.Date!.Value > today || d.Date.Value < oldest))
        {
            throw AppException.BadRequest(AppConstants.ErrorCodes.ActivityDateOutOfRange);
        }

        var dates = days.Select(d => d.Date!.Value).ToList();
        var existing = (await activities.GetDaysAsync(employeeId, dates, cancellationToken))
            .ToDictionary(a => a.Date);

        foreach (var day in days)
        {
            var date = day.Date!.Value;
            if (!existing.TryGetValue(date, out var activity))
            {
                activity = DailyActivity.Create(employeeId, date);
                activities.Add(activity);
            }

            var source = string.IsNullOrWhiteSpace(day.Source) ? null : day.Source.Trim();
            var isFlagged = day.Steps > FlagStepsAbove || (source is null && day.Steps > 0);

            activity.Update(day.Steps, day.DistanceMeters, day.ActiveMinutes, day.CaloriesKcal, source, isFlagged, now);
        }

        await activities.SaveChangesAsync(cancellationToken);

        return await activities.ListAsync(employeeId, dates.Min(), dates.Max(), cancellationToken);
    }

    public async Task<IReadOnlyList<ActivityDayResponse>> ListAsync(
        Guid employeeId,
        DateOnly? startDate,
        DateOnly? endDate,
        CancellationToken cancellationToken)
    {
        await EnsureEmployeeExistsAsync(employeeId, cancellationToken);

        var end = endDate ?? QatarTimeHelper.ToQatarDate(clock.GetUtcNow());
        var start = startDate ?? end.AddDays(-(MaxDaysBack - 1));

        if (start > end || end.DayNumber - start.DayNumber >= MaxRangeDays)
        {
            throw AppException.BadRequest(AppConstants.ErrorCodes.ActivityDateOutOfRange);
        }

        return await activities.ListAsync(employeeId, start, end, cancellationToken);
    }

    private async Task EnsureEmployeeExistsAsync(Guid employeeId, CancellationToken cancellationToken)
    {
        if (await employees.GetByIdAsync(employeeId, cancellationToken) is null)
        {
            throw AppException.NotFound(AppConstants.ErrorCodes.EmployeeNotFound);
        }
    }
}
