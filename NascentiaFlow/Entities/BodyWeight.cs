using NodaTime;

namespace NascentiaFlow.Entities;

public class BodyWeight : EntityBase
{
    public Instant Date { get; set; }
    
    public float Weight { get; set; }
}