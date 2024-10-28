namespace Not.Filesystem;

public abstract class FilesystemContext
{
    protected string GetAppDirectory(string subdirectory)
    {
        //return Path.Combine(
        //    "D:\\Source\\NTS\\not-timing-system\\src-v2\\Judge\\NTS.Judge.MAUI\\bin\\Release\\net8.0-windows10.0.19041.0\\win10-x64",
        //    subdirectory);
        var basePath =
#if DEBUG
            "C:\\tmp\\nts";
#else
            Directory.GetCurrentDirectory();
#endif
        return Path.Combine(basePath, subdirectory);
    }
}
