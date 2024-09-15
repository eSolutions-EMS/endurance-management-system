using Not.Domain.Ports;
using Not.Filesystem;
using Not.Logging.Filesystem;
using Not.Storage.Ports;
using Not.Storage.StaticOptions;

namespace NTS.Judge.Startup;

public class JudgeContext : FilesystemContext, IFilesystemLoggerConfiguration, IFileStorageConfiguration, IStaticOptionsConfiguration
{
    string IStaticOptionsConfiguration.Path => GetAppDirectory("config");
    string IFilesystemLoggerConfiguration.Directory => GetAppDirectory("logs");
    string IFileStorageConfiguration.Path => GetAppDirectory("data");
}

