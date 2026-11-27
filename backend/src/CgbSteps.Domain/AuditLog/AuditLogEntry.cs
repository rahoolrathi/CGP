using CgbSteps.Domain.Common;

namespace CgbSteps.Domain.AuditLog;

public sealed class AuditLogEntry : Entity
{
    public const int MaxCodeLength = 100;

    private AuditLogEntry()
    {
    }

    public DateTimeOffset OccurredAt { get; private init; }
    public Guid? ActorId { get; private init; }
    public string Action { get; private init; } = string.Empty;
    public string TargetType { get; private init; } = string.Empty;
    public string? TargetId { get; private init; }

    public static AuditLogEntry Record(
        Guid? actorId,
        string action,
        string targetType,
        string? targetId,
        DateTimeOffset occurredAt)
    {
        return new AuditLogEntry
        {
            ActorId = actorId,
            Action = RequireCode(action, nameof(action)),
            TargetType = RequireCode(targetType, nameof(targetType)),
            TargetId = targetId,
            OccurredAt = occurredAt.ToUniversalTime(),
        };
    }

    private static string RequireCode(string value, string paramName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, paramName);
        if (value.Length > MaxCodeLength)
        {
            throw new ArgumentException($"Must be at most {MaxCodeLength} characters.", paramName);
        }

        return value;
    }
}
