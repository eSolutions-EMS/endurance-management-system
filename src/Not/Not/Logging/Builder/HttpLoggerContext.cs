using Not.Injection.Config;

namespace Not.Logging.Builder;

public class HttpLoggerContext : NConfig
{
    protected override string[] RequiredFields => [nameof(Host)];
    public string? Host { get; set; }
}
