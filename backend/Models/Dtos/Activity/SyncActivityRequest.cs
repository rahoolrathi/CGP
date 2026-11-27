using System.ComponentModel.DataAnnotations;

namespace CgbSteps.Api.Models.Dtos.Activity;

/// <summary>Daily totals to save. Sending the same day again replaces it, so retries are safe.</summary>
public sealed class SyncActivityRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(31)]
    public IReadOnlyList<ActivityDayRequest> Days { get; init; } = [];
}
