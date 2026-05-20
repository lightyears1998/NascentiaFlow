namespace NascentiaFlow.Entities;

public enum EditionFormat
{
    JSON,
    BLOB
}

public class Edition : EntityBase
{
    public EditionFormat Format { set; get; }

    // public byte[] Data { set; get; } = [];
}
