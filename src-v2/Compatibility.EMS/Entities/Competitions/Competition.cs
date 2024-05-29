using NTS.Compatibility.EMS.Entities.Laps;
using System.Data;
using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Competitions;

public class Competition : DomainBase<CompetitionException>
{
    private Competition() {}
    public Competition(CompetitionType type, string name) : base(GENERATE_ID)
    {
        this.Type = type;
        this.Name = name;
        this.StartTime = DateTime.Now;
    }

    private List<Lap> laps = new();
    public CompetitionType Type { get; internal set; }
    public string Name { get; internal set; }
    public string FeiId { get; internal set; } = string.Empty;
    public string FeiScheduleNumber { get; internal set; } = string.Empty;
    public string Rule { get; internal set; } = string.Empty;
    public DateTime StartTime { get; set; }

    public void Save(Lap lap)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Lap> Laps
    {
        get => this.laps.OrderBy(x => x.OrderBy).ToList().AsReadOnly();
        private set { this.laps = value.ToList(); }
    }
}
public enum CompetitionType
{
    Invalid = 0,
    National = 1,
    International = 2,
}
