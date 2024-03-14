using NTS.Domain.Events.Start;
using NTS.Domain.Setup.Import;
using System.Text.Json.Serialization;

namespace NTS.Domain.Setup.Entities;

public class Competition : DomainEntity, ISummarizable, IImportable
{

    public static Competition Create(CompetitionType type, DateTime start) => new(type, start);
    public static Competition Update(CompetitionType type, DateTime start, List<Loop> loops, List<Starter> starters) => new(type, start, loops, starters);

    [JsonConstructor]
    public Competition(CompetitionType type, DateTime start, List<Loop> loops, List<Starter> starters)
    {
        this.Type = type;
        this.Start = new DateTimeOffset(start);
        this.Loops = loops;
        this.Starters = starters;
    }

    public Competition(CompetitionType type, DateTime start)
    {
        this.Type = type;
		this.Start = new DateTimeOffset(start);
    }

    public CompetitionType Type { get; private set; }
	public DateTimeOffset Start { get; private set; }
	public List<Loop> Loops { get; private set; } = new();
	public List<Starter> Starters { get; private set; } = new();

	public string Summarize()
	{
		var summary = new Summarizer(this);
		summary.Add("Loops".Localize(), this.Loops);
		summary.Add("Starters".Localize(), this.Starters);
		return summary.ToString();
	}
	public override string ToString()
	{
		var sb = new StringBuilder();
		sb.Append($"{LocalizationHelper.Get(this.Type)}, {"Loops".Localize()}: {this.Loops.Count}, {"Starters".Localize()}: {this.Starters.Count}, ");
		sb.Append($"{"Start".Localize()}: {this.Start.ToString("f", CultureInfo.CurrentCulture)}");
		return sb.ToString();
	}
}

public enum CompetitionType
{
	Qualification = 1,
	National = 2,
	International = 3,
}
