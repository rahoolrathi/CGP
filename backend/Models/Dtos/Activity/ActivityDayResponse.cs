namespace CgbSteps.Api.Models.Dtos.Activity;

public sealed record ActivityDayResponse(
    DateOnly Date,
    int Steps,
    double DistanceMeters,
    int ActiveMinutes,
    double CaloriesKcal,
    bool IsFlagged);
