using Not.Logging.HTTP;

namespace Not.Logging.Builder;

public class HttpLoggerContext : IHttpLoggerConfiguration
{
    public string? Host { get; set; }

    internal void Validate()
    {
        if (Host == null)
        {
            // TODO: extract common context and common confugaration in extensions?
            throw new ApplicationException($"{nameof(HttpLoggerContext)} is not configured: {nameof(Host)} is required");
        }
    }
}
