using Not.Logging.Filesystem;

namespace Not.Logging.Builder;

public class FilesystemLoggerContext : IFilesystemLoggerConfiguration
{
    public string? Path { get; set; }

    public void Validate()
    {
        if (Path == null)
        {
            throw new ApplicationException($"{nameof(HttpLoggerContext)} is not configured: {nameof(Path)} is required");
        }
    }
}
