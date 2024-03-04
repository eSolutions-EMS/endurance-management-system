using NTS.Domain.Setup.Import;

namespace NTS.Domain.Setup.Entities;

public class Starter : DomainEntity, ISummarizable, IImportable
{
    public Starter(int number)
    {
        this.Number = number;
    }

    public int Number { get; private set; }
    public Athlete? Athlete { get; private set; }
    public Horse? Horse { get; private set; }
    public List<IdTag> Tags { get; private set; } = new();

    public string Summarize()
    {
        var summary = new Summarizer(this);
        summary.Add("Tags".Localize(), this.Tags);
        return summary.ToString();
    }

	public override string ToString()
	{
        return $"{"#".Localize()}{this.Number}: {this.Athlete}, {this.Horse}";
	}
}
