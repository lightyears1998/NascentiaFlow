using Microsoft.EntityFrameworkCore;

namespace NascentiaFlow.Entities;

[Index(nameof(Key), IsUnique = true)]
public class KeyValueStore : EntityBase
{
    public string Key { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}
