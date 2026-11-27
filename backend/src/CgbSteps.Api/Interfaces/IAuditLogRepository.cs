using CgbSteps.Api.Models.Entities;

namespace CgbSteps.Api.Interfaces;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLogEntry entry, CancellationToken cancellationToken);
}
