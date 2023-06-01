namespace EMS.Judge.Application.Services;

public class Settings : ISettings
{
    public bool IsConfigured { get; set; }
    public bool IsSandboxMode { get; set; }
    public bool ShouldUseServer { get; set; }
    public bool ShouldUseVupRfid { get; set; }
    public string Version { get; set; }
}

public interface ISettings
{
    bool IsConfigured { get; }
    bool IsSandboxMode { get; }
    bool ShouldUseServer { get; }
    bool ShouldUseVupRfid { get; }
    string Version { get; }
}
