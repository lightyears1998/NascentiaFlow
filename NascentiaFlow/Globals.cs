using System.Threading;

namespace NascentiaFlow;

public static class Globals
{
    public static ManualResetEvent DatabaseIsReady { get; } = new (false);
}
