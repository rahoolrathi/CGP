using CgbSteps.Api.Models.Dtos.Activity;
using CgbSteps.Api.Models.Entities;

namespace CgbSteps.Api.Repositories.Interfaces;

public interface IActivityRepository
{
    Task<List<DailyActivity>> GetDaysAsync(
        Guid employeeId,
        IReadOnlyCollection<DateOnly> dates,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ActivityDayResponse>> ListAsync(
        Guid employeeId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken);

    void Add(DailyActivity activity);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
