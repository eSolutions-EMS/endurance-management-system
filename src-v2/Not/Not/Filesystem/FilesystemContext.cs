namespace Not.Filesystem;

public abstract class FilesystemContext
{
    protected string GetAppDirectory(string subdirectory)
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
