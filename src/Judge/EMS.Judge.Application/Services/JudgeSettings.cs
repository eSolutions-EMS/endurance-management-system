namespace EMS.Judge.Application.Services;

public class JudgeSettings : ISettings
{
    public bool IsConfigured { get; set; }
    public bool IsSandboxMode { get; set; }
    public bool StartServer { get; set; }
    public bool StartVupRfid { get; set; }
    public string Version { get; set; }
}

public interface ISettings
{
    bool IsConfigured { get; }
    bool IsSandboxMode { get; }
    bool StartServer { get; }
    bool StartVupRfid { get; }
    string Version { get; }
}
