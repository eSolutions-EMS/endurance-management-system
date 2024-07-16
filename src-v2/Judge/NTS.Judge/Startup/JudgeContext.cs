using Not.Logging.Filesystem;
using Not.Storage.Ports;

namespace NTS.Judge.Startup;

public class JudgeContext : IFilesystemLoggerConfiguration, IFileStorageConfiguration
{
    string IFilesystemLoggerConfiguration.Directory => GetAppDirectory("logs");
    string IFileStorageConfiguration.Path => GetAppDirectory("data");

    private string GetAppDirectory(string subdirectory)
    {
        var basePath =
#if DEBUG
            "C:\\tmp\\nts";
#else
            Directory.GetCurrentDirectory();
#endif
        return Path.Combine(basePath, subdirectory);
    }
}
