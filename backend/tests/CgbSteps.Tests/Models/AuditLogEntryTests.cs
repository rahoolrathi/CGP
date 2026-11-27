using CgbSteps.Api.Models.Entities;

namespace CgbSteps.Tests.Models;

public sealed class AuditLogEntryTests
{
    private static readonly DateTimeOffset QatarMorning = new(2026, 11, 27, 9, 0, 0, TimeSpan.FromHours(3));

    [Fact]
    public void Create_StoresTimestampInUtc()
    {
        var entry = AuditLogEntry.Create(Guid.NewGuid(), "employee.created", "employee", "E123", QatarMorning);

        Assert.Equal(TimeSpan.Zero, entry.OccurredAt.Offset);
        Assert.Equal(QatarMorning, entry.OccurredAt);
        Assert.NotEqual(Guid.Empty, entry.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_RejectsBlankAction(string action)
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            AuditLogEntry.Create(null, action, "employee", null, QatarMorning));
    }

    [Fact]
    public void Create_RejectsOverlongTargetType()
    {
        var tooLong = new string('x', AuditLogEntry.MaxCodeLength + 1);

        Assert.Throws<ArgumentException>(() =>
            AuditLogEntry.Create(null, "employee.created", tooLong, null, QatarMorning));
    }
}
