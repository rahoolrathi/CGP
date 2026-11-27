using CgbSteps.Api.Data;
using CgbSteps.Api.Models.Dtos.Activity;
using CgbSteps.Api.Models.Entities;
using CgbSteps.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CgbSteps.Api.Repositories;

public sealed class ActivityRepository(AppDbContext db) : IActivityRepository
{
    public Task<List<DailyActivity>> GetDaysAsync(
        Guid employeeId,
        IReadOnlyCollection<DateOnly> dates,
        CancellationToken cancellationToken) =>
        db.DailyActivities
            .Where(a => a.EmployeeId == employeeId && dates.Contains(a.Date))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<ActivityDayResponse>> ListAsync(
        Guid employeeId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken) =>
        await db.DailyActivities
            .AsNoTracking()
            .Where(a => a.EmployeeId == employeeId && a.Date >= startDate && a.Date <= endDate)
            .OrderBy(a => a.Date)
            .Select(a => new ActivityDayResponse(
                a.Date, a.Steps, a.DistanceMeters, a.ActiveMinutes, a.CaloriesKcal, a.IsFlagged))
            .ToListAsync(cancellationToken);

    public void Add(DailyActivity activity) => db.DailyActivities.Add(activity);

    public Task SaveChangesAsync(CancellationToken cancellationToken) => db.SaveChangesAsync(cancellationToken);
}
