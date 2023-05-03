namespace EnduranceJudge.Application.Services;

public class Settings : ISettings
{
    public bool IsConfigured { get; set; }
    public bool IsSandboxMode { get; set; }
    public string Version { get; set; }
}

public interface ISettings
{
    bool IsConfigured { get; }
    bool IsSandboxMode { get; }
    string Version { get; }
}
