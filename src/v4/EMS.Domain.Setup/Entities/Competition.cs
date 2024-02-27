using NTS.Domain.Events.Start;
using NTS.Domain.Setup.Import;

namespace NTS.Domain.Setup.Entities;

public class Competition : DomainEntity, ISummarizable, IImportable
{
    public Competition(CompetitionType type, DateTime start)
    {
        this.Type = type;
		this.Start = new Timestamp(start);
    }

    public CompetitionType Type { get; private set; }
	public Timestamp Start { get; private set; }
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
