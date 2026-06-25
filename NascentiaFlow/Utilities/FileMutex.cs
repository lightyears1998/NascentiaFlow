using System.IO;

namespace NascentiaFlow.Utilities;

public sealed class FileMutex : IDisposable
{
    private readonly FileStream _lockStream;

    public FileMutex(string filePath)
    {
        _lockStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
    }

    public void Dispose()
    {
        _lockStream.Dispose();
    }
}
