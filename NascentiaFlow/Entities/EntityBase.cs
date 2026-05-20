using NodaTime;
using ReactiveUI;

namespace NascentiaFlow.Entities;

public class EntityBase : ReactiveObject
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid EditionId { get; set; }

    public Instant EditionTimestamp { get; set; }
}

public static class EntityBaseExtensions
{
    public static void UpdateEdition(this EntityBase x)
    {
        x.EditionId = Guid.NewGuid();
        x.EditionTimestamp = SystemClock.Instance.GetCurrentInstant();
    }
}
