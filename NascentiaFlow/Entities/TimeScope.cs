using NodaTime;

namespace NascentiaFlow.Entities;

public class TimeScope
{
    public string Name { set; get; } = string.Empty;

    public string Description { set; get; } = string.Empty;

    public Instant StartedAt { get; set; }

    public Instant EndedAt { get; set; }

    public Duration Duration => EndedAt - StartedAt;
}

public class TimeScopeTemplate
{
    public string Name { set; get; } = string.Empty;

    public string Description { set; get; } = string.Empty;

    public LocalTime StartedAt { get; set; }

    public LocalTime EndedAt { get; set; }

    public Duration Duration
    {
        get
        {
            var p = Period.Between(StartedAt, EndedAt).ToDuration().TotalSeconds;
            return Duration.FromSeconds(Math.Abs(p));
        }
    }
}
