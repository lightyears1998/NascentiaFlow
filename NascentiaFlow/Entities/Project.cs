using NodaTime;

namespace NascentiaFlow.Entities;

public class Project : EntityBase
{
    public string Name { set; get; } = string.Empty;

    public string Description { set; get; } = string.Empty;

    public string Category { set; get; } = string.Empty;

    public Instant CreatedAt { set; get; }
}
