using CgbSteps.Api.Interfaces;
using CgbSteps.Api.Models.Entities;
using CgbSteps.Api.Services;
using Microsoft.Extensions.Time.Testing;

namespace CgbSteps.Tests.Services;

public sealed class AuditLogServiceTests
{
    private sealed class InMemoryAuditLogRepository : IAuditLogRepository
    {
        public List<AuditLogEntry> Entries { get; } = [];

        public Task AddAsync(AuditLogEntry entry, CancellationToken cancellationToken)
        {
            Entries.Add(entry);
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task RecordAsync_SavesEntryStampedWithCurrentTime()
    {
        var repository = new InMemoryAuditLogRepository();
        var clock = new FakeTimeProvider(new DateTimeOffset(2026, 11, 27, 6, 0, 0, TimeSpan.Zero));
        var service = new AuditLogService(repository, clock);
        var actorId = Guid.NewGuid();

        await service.RecordAsync(actorId, "badge.updated", "badge", "7", TestContext.Current.CancellationToken);

        var entry = Assert.Single(repository.Entries);
        Assert.Equal(actorId, entry.ActorId);
        Assert.Equal("badge.updated", entry.Action);
        Assert.Equal(clock.GetUtcNow(), entry.OccurredAt);
    }
}
