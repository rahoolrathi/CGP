namespace CgbSteps.Api.Models.Dtos;

public sealed record SystemInfoResponse(string Version, DateTimeOffset ServerTimeUtc, DateOnly QatarDate);
