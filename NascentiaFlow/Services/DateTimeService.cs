using NodaTime;

namespace NascentiaFlow.Services;

public class DateTimeService
{
    // TODO 从 ServiceProvider 处通过 ctor 或 IOptions 注入
    private static readonly DateTimeZone _zone = DateTimeZoneProviders.Tzdb.GetSystemDefault();

    public static ZonedDateTime GetToday()
    {
        var now = SystemClock.Instance.GetCurrentInstant();
        var zonedDateTime = now.InZone(_zone);
        return zonedDateTime;
    }
}
