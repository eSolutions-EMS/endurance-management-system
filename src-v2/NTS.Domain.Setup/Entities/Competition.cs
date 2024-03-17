using NTS.Domain.Events.Start;
using NTS.Domain.Setup.Import;
using System.Text.Json.Serialization;

namespace NTS.Domain.Setup.Entities;

public class Competition : DomainEntity, ISummarizable, IImportable
{

    public static Competition Create(string name, CompetitionType type, DateTime start) => new(name,type, start);
    public static Competition Update(string name, CompetitionType type, DateTime start, List<Loop> loops, List<Contestant> contestants) => new(name, type, start, loops, contestants);

    [JsonConstructor]
    public Competition(string name, CompetitionType type, DateTime start, List<Loop> loops, List<Contestant> contestants)
    {
        this.Name = name;
        this.Type = type;
        this.StartTime = new DateTimeOffset(start);
        this.Loops = loops;
        this.Contestants = contestants;
    }

    public Competition(string Name, CompetitionType type, DateTime start)
    {
        this.Name = Name;
        this.Type = type;
		this.StartTime = new DateTimeOffset(start);
    }
    public string Name { get; private set; }
    public CompetitionType Type { get; private set; }
	public DateTimeOffset StartTime { get; private set; }
    public List<Loop> Loops { get; private set; }
    public List<Contestant> Contestants { get; private set; }

	public string Summarize()
	{
		var summary = new Summarizer(this);
		summary.Add("Loops".Localize(), this.Loops);
		summary.Add("Contestants".Localize(), this.Contestants);
		return summary.ToString();
	}
	public override string ToString()
	{
		var sb = new StringBuilder();
		sb.Append($"{LocalizationHelper.Get(this.Type)}, {"Loops".Localize()}: {this.Loops.Count}, {"Starters".Localize()}: {this.Contestants.Count}, ");
		sb.Append($"{"Start".Localize()}: {this.StartTime.ToString("f", CultureInfo.CurrentCulture)}");
		return sb.ToString();
	}
    public void AddDefaultLists()
    {
        this.Loops = new List<Loop>();
        this.Contestants = new List<Contestant>();  
    }
}

public enum CompetitionType
{
	FEI = 1,
	National = 2
}
