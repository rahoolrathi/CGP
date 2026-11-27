using System.ComponentModel.DataAnnotations;

namespace CgbSteps.Api.Models.Dtos.Activity;

public sealed class ActivityDayRequest
{
    /// <summary>The day in Qatar time (yyyy-MM-dd).</summary>
    [Required]
    public DateOnly? Date { get; init; }

    [Range(0, 200_000)]
    public int Steps { get; init; }

    [Range(0, 500_000)]
    public double DistanceMeters { get; init; }

    [Range(0, 1_440)]
    public int ActiveMinutes { get; init; }

    [Range(0, 20_000)]
    public double CaloriesKcal { get; init; }

    /// <summary>Package name of the app that recorded the steps (Health Connect data origin).</summary>
    [StringLength(100)]
    public string? Source { get; init; }
}
