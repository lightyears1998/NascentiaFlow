using NodaTime;

namespace NascentiaFlow.Entities;

public class Focus : EntityBase
{
    public string Brief { set; get; } = string.Empty;

    public string Description { set; get; } = string.Empty;

    public ActivityType? ActivityType { set; get; }

    public Instant? StartedAt { set; get; }

    public Instant? EndedAt { set; get; }

    public float? AvailablePoints { set; get; }

    public float? GainedPoints { set; get; }
}

public enum FocusDurationType
{
    Focusing,
    Interrupted
}

public class FocusDuration : EntityBase
{
    public Focus? Focus { set; get; }

    public Instant StartedAt { set; get; }

    public Instant? EndedAt { set; get; }
}
