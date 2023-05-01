namespace EnduranceJudge.Application.Services;

public class SettingsService : ISettings
{
    public bool IsConfigured { get; set; }
    public bool IsSandboxMode { get; set; }
}

public interface ISettings
{
    bool IsConfigured { get; }
    bool IsSandboxMode { get; }
}
