namespace NascentiaFlow.Entities;

public class Context : EntityBase
{
    public TimeScopeTemplate? DefaultTimeScope { set; get; }
    public Location? DefaultLocation { set; get; }
}
