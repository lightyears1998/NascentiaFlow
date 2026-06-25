using System.Threading;
using NascentiaFlow.Utilities;

namespace NascentiaFlow;

public static class Globals
{
    public static ManualResetEvent DatabaseIsReady { get; } = new (false);

    public static FileMutex? AppInstanceMutex { get; set; }
}
