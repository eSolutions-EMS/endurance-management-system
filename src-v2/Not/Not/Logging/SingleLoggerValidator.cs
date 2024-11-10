using Serilog;

namespace Not.Logging;

internal static class SingleLoggerValidator
{
    public static void Validate()
    {
        if (_isLoggerConfigured)
        {
            throw new Exception(
                "Serilog static logger is already configured. Second logger is unsupported as it will simply replace the original"
            );
        }
        _isLoggerConfigured = true;
    }

    static bool _isLoggerConfigured;
}
