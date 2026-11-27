namespace CgbSteps.Api.Helpers;

public static class QatarTimeHelper
{
    public static readonly TimeZoneInfo Zone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Qatar");

    public static DateOnly ToQatarDate(DateTimeOffset instant) =>
        DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(instant, Zone).DateTime);

    public static (DateTimeOffset StartUtc, DateTimeOffset EndUtc) DayBoundsUtc(DateOnly qatarDate)
    {
        var midnight = qatarDate.ToDateTime(TimeOnly.MinValue);
        var start = new DateTimeOffset(midnight, Zone.GetUtcOffset(midnight)).ToUniversalTime();
        return (start, start.AddDays(1));
    }
}
