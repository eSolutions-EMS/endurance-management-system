using NTS.Compatibility.EMS.Abstractions;
using NTS.Compatibility.EMS.Entities.Laps;

namespace NTS.Compatibility.EMS.Entities.Competitions;

public class Competition : DomainBase<CompetitionException>, ICompetitionState
{
    private Competition() {}
    public Competition(CompetitionType type, string name) : base(GENERATE_ID)
    {
        this.Type = type;
        this.Name = name;
        this.StartTime = DateTime.Now;
    }
    public Competition(ICompetitionState state) : base(GENERATE_ID)
    {
        this.Type = state.Type;
        this.Name = state.Name;
        this.StartTime = state.StartTime;
    }

    private List<Lap> laps = new();
    public CompetitionType Type { get; internal set; }
    public string Name { get; internal set; }
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
