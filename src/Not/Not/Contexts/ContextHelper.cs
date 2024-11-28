using Not.Exceptions;

namespace Not.Contexts;

public static class ContextHelper
{
    public static void SetApplicationName(string applicatioName)
    {
        _applicationName = applicatioName;
    }

    public static string GetAppDirectory(string subdirectory)
    {
        GuardHelper.ThrowIfDefault(_applicationName);

        var basePath =
#if DEBUG
            $"C:\\tmp\\{_applicationName}";
#else
        Directory.GetCurrentDirectory();
#endif
        return Path.Combine(basePath, subdirectory);
    }

    static string? _applicationName;
}
