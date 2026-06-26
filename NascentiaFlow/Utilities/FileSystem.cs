using System.IO;

namespace NascentiaFlow.Utilities;

public static class FileSystem
{
    public static void EnsureDirs(string[] dirs)
    {
        foreach (var dir in dirs)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
