namespace CgbSteps.Api.Models.Entities;

/// <summary>One row per employee per day (Qatar date), with totals read from Health Connect.</summary>
public sealed class DailyActivity
{
    private DailyActivity()
    {
    }

    public Guid Id { get; private set; } = Guid.CreateVersion7();

    public Guid EmployeeId { get; private set; }

    public DateOnly Date { get; private set; }

    public int Steps { get; private set; }

    public double DistanceMeters { get; private set; }

    public int ActiveMinutes { get; private set; }

    public double CaloriesKcal { get; private set; }

    /// <summary>The app that recorded the data, for example com.google.android.apps.fitness.</summary>
    public string? Source { get; private set; }

    /// <summary>True when the values look implausible and an admin should review them.</summary>
    public bool IsFlagged { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static DailyActivity Create(Guid employeeId, DateOnly date) => new()
    {
        EmployeeId = employeeId,
        Date = date,
    };

    public void Update(
        int steps,
        double distanceMeters,
        int activeMinutes,
        double caloriesKcal,
        string? source,
        bool isFlagged,
        DateTimeOffset now)
    {
        Steps = steps;
        DistanceMeters = distanceMeters;
        ActiveMinutes = activeMinutes;
        CaloriesKcal = caloriesKcal;
        Source = source;
        IsFlagged = isFlagged;
        UpdatedAt = now;
    }
}
