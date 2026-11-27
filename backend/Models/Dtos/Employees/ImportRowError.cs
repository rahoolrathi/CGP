namespace CgbSteps.Api.Models.Dtos.Employees;

/// <summary>A CSV row that was not imported. Row 1 is the header, so data starts at row 2.</summary>
public sealed record ImportRowError(int Row, string Code);
