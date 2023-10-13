using EMS.Domain.Events.Start;
using EMS.Domain.Setup.Import;

namespace EMS.Domain.Setup.Entities;

public class Horse : DomainEntity, ISummarizable, IImportable
{
    public Horse(string name)
    {
        this.Name = name;
    }

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
