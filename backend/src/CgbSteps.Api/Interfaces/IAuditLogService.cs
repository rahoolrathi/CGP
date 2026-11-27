namespace CgbSteps.Api.Interfaces;

public interface IAuditLogService
{
    Task RecordAsync(
        Guid? actorId,
        string action,
        string targetType,
        string? targetId,
        CancellationToken cancellationToken);
}
