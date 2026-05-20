using NodaTime;

namespace NascentiaFlow.Entities;

public class Stuff : EntityBase
{
    public string Name { set; get; } = string.Empty;

    public string Description { set; get; } = string.Empty;

    public Instant CollectedAt { set; get; }
}
