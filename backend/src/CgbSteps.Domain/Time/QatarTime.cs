namespace CgbSteps.Domain.Time;

public static class QatarTime
{
    public static readonly TimeZoneInfo Zone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Qatar");

    public static DateOnly ToQatarDate(DateTimeOffset instant) =>
        DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(instant, Zone).DateTime);

    public static (DateTimeOffset StartUtc, DateTimeOffset EndUtc) DayBoundsUtc(DateOnly qatarDate)
    {
        var offset = Zone.GetUtcOffset(qatarDate.ToDateTime(TimeOnly.MinValue));
        var start = new DateTimeOffset(qatarDate.ToDateTime(TimeOnly.MinValue), offset).ToUniversalTime();
        return (start, start.AddDays(1));
    }
}
