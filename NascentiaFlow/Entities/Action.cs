namespace NascentiaFlow.Entities;

public class Action : EntityBase
{
    public Project? Project { set; get; }

    public string Description { set; get; } = string.Empty;
}

public class ActionType : EntityBase
{
    public string Name { set; get; } = string.Empty;

    public string Description { set; get; } = string.Empty;

    public string Shortcut { set; get; } = string.Empty;
}
