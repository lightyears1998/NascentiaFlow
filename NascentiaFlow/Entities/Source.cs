using NodaTime;

namespace NascentiaFlow.Entities;

public class Source : EntityBase
{
    public string Name { set; get; } = string.Empty;

    public string Description { set; get; } = string.Empty;

    public Instant CreatedAt { set; get; }

    public Instant? LastCheckedAt { set; get; }
}
