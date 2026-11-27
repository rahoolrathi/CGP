using CgbSteps.Api.Interfaces;
using CgbSteps.Api.Models.Entities;

namespace CgbSteps.Api.Services;

public sealed class AuditLogService(IAuditLogRepository repository, TimeProvider clock) : IAuditLogService
{
    public Task RecordAsync(
        Guid? actorId,
        string action,
        string targetType,
        string? targetId,
        CancellationToken cancellationToken)
    {
        var entry = AuditLogEntry.Create(actorId, action, targetType, targetId, clock.GetUtcNow());
        return repository.AddAsync(entry, cancellationToken);
    }
}
