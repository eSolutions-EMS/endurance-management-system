using NTS.Domain.Setup.Import;

namespace NTS.Domain.Setup.Entities;

public class Horse : DomainEntity, ISummarizable, IImportable
{
    public Horse(string name)
    {
        this.Name = name;
    }

	public string? FeiId { get; private set; }
	public string Name { get; private set; }

	public string Summarize()
	{
        return this.ToString();
	}
	public override string ToString()
	{
		return this.Name;
	}
}
