using CgbSteps.Api.Data;
using CgbSteps.Api.Interfaces;
using CgbSteps.Api.Models.Entities;

namespace CgbSteps.Api.Repositories;

public sealed class AuditLogRepository(AppDbContext db) : IAuditLogRepository
{
    public async Task AddAsync(AuditLogEntry entry, CancellationToken cancellationToken)
    {
        db.AuditLog.Add(entry);
        await db.SaveChangesAsync(cancellationToken);
    }
}
