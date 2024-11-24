using Not.Filesystem;
using Not.Logging.Filesystem;
using Not.Storage.Stores.Files.Ports;
using Not.Storage.Stores.StaticOptions.Ports;

namespace NTS.Judge.Shared;

public class JudgeContext
    : FilesystemContext,
        IFilesystemLoggerConfiguration,
        IFileStorageConfiguration,
        IStaticOptionsConfiguration
{
    string IStaticOptionsConfiguration.Path => GetAppDirectory("resources");
    string IFilesystemLoggerConfiguration.Directory => GetAppDirectory("logs");
    string IFileStorageConfiguration.Path => GetAppDirectory("data");
}
