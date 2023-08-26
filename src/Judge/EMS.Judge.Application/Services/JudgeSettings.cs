namespace EMS.Judge.Application.Services;

public class JudgeSettings : ISettings
{
    public bool IsConfigured { get; set; }
    public bool IsSandboxMode { get; set; }
    public bool StartServer { get; set; }
    public bool StartVupRfid { get; set; }
    public string Version { get; set; }
    public bool UseVD67InManager { get; set; }
    public string WitnessEventType { get; set; }
}

public interface ISettings
{
    bool IsConfigured { get; }
    bool IsSandboxMode { get; }
    bool StartServer { get; }
    bool StartVupRfid { get; }
    bool UseVD67InManager { get; }
    string Version { get; }
    string WitnessEventType { get; set; }
}
