using NTS.Domain.Events.Start;
using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Competition : DomainEntity, ISummarizable, IImportable
{

    public static Competition Create(string name, CompetitionType type, DateTimeOffset start) => new(name,type, start);
    public static Competition Update(string name, CompetitionType type, DateTimeOffset start) => new(name, type, start);

    private List<Loop> _loops = new();
    private List<Contestant> _contestants = new();

    [JsonConstructor]
    public Competition(string name, CompetitionType type, DateTimeOffset start)
    {
        this.Name = name;
        this.Type = type;
        this.StartTime = start;
    }
    public string Name { get; private set; }
    public CompetitionType Type { get; private set; }
	public DateTimeOffset StartTime { get; private set; }

    public IReadOnlyList<Loop> Loops
    {
        get => _loops.AsReadOnly();
        private set => _loops = value.ToList();
    }
    public IReadOnlyList<Contestant> Contestants
    {
        get => _contestants.AsReadOnly();
        private set => _contestants = value.ToList();
    }

   
    public string Summarize()
	{
		var summary = new Summarizer(this);
		summary.Add("Loops".Localize(), this._loops);
		summary.Add("Contestants".Localize(), this._contestants);
		return summary.ToString();
	}
	public override string ToString()
	{
		var sb = new StringBuilder();
		sb.Append($"{LocalizationHelper.Get(this.Type)}, {"Loops".Localize()}: {this.Loops.Count}, {"Starters".Localize()}: {this.Contestants.Count}, ");
		sb.Append($"{"Start".Localize()}: {this.StartTime.ToString("f", CultureInfo.CurrentCulture)}");
		return sb.ToString();
	}

}

public enum CompetitionType
{
	FEI = 1,
	National = 2
}
