using System.Data;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsCompetition : EmsDomainBase<EmsCompetitionException>, IEmsCompetitionState
{
    private EmsCompetition() { }
    public EmsCompetition(EmsCompetitionType type, string name) : base(default)
    {
        Type = type;
        Name = name;
        StartTime = DateTime.Now;
    }
    public EmsCompetition(IEmsCompetitionState state, IEnumerable<EmsLap> laps) : base(default)
    {
        Type = state.Type;
        Name = state.Name;
        StartTime = state.StartTime;
        FeiScheduleNumber = state.FeiScheduleNumber;
        FeiCategoryEventNumber = state.FeiCategoryEventNumber;
        EventCode = state.EventCode;
        Rule = state.Rule;
        laps = laps.ToList();
    }

    private List<EmsLap> laps = new();
    public EmsCompetitionType Type { get; internal set; }
    public string Name { get; internal set; }
    /// <summary>
    /// Used to build EnduranceEvent.FEIID and Competition.FEIID. Its located at EVENT DETAIL (Search Venue->Show->ShowName->EventCode) upper right, https://data.fei.org
    /// </summary>
    public string FeiCategoryEventNumber { get; internal set; }
    /// <summary>
    /// Used to Competition.FEIID. Its located at COMPETITION DETAIL (Search Venue->Show->ShowName->EventCode->CompetitionName), upper right, https://data.fei.org
    /// </summary>
    public string FeiScheduleNumber { get; internal set; } = string.Empty;
    public string Rule { get; internal set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public string EventCode { get; internal set; }

    public IReadOnlyList<EmsLap> Laps
    {
        get => laps.OrderBy(x => x.OrderBy).ToList().AsReadOnly();
        private set { laps = value.ToList(); }
    }
}
