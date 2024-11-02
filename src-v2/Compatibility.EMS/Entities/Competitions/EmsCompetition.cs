using System.Data;
using NTS.Compatibility.EMS.Abstractions;
using NTS.Compatibility.EMS.Entities.Laps;

namespace NTS.Compatibility.EMS.Entities.Competitions;

public class EmsCompetition : EmsDomainBase<EmsCompetitionException>
{
    [Newtonsoft.Json.JsonConstructor]
    private EmsCompetition() { }

    public EmsCompetition(EmsCompetitionType type, string name)
        : base(GENERATE_ID)
    {
        this.Type = type;
        this.Name = name;
        this.StartTime = DateTime.Now;
    }

    public EmsCompetition(IEmsCompetitionState state)
        : base(GENERATE_ID)
    {
        this.Type = state.Type;
        this.Name = state.Name;
        this.StartTime = state.StartTime;
        FeiScheduleNumber = state.FeiScheduleNumber;
        FeiCategoryEventNumber = state.FeiCategoryEventNumber;
        EventCode = state.EventCode;
        Rule = state.Rule;
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
    public string EventCode { get; internal set; }
    public string Rule { get; internal set; } = string.Empty;
    public DateTime StartTime { get; set; }

    public void Save(EmsLap lap)
    {
        if (laps.Contains(lap))
        {
            laps.Remove(lap);
        }
        laps.Add(lap);
    }

    public IReadOnlyList<EmsLap> Laps
    {
        get => this.laps.OrderBy(x => x.OrderBy).ToList().AsReadOnly();
        private set { this.laps = value.ToList(); }
    }
}

public enum EmsCompetitionType
{
    Invalid = 0,
    National = 1,
    International = 2,
}
