using NodaTime;
using NodaTime.Calendars;

namespace NascentiaFlow.Services;

public class ChronicleService
{
    public static ZonedDateTime GetToday()
    {
        var now = SystemClock.Instance.GetCurrentInstant();
        var zone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
        var zonedDateTime = now.InZone(zone);
        return zonedDateTime;
    }
}
