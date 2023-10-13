using EMS.Domain.Events.Start;
using EMS.Domain.Setup.Objects;

namespace EMS.Domain.Setup.Entities;

public class IdTag : DomainEntity, ISummarizable, ICoreIdentifier
{
	private readonly NumberCoreIdentifier coreId;

    public IdTag(string tagId, string position, int number)
    {
		this.TagId = tagId;
		this.Position = position;
		this.coreId = new NumberCoreIdentifier(number);
    }

    public string TagId { get; private set; }
	public string Position { get; private set; }

	public bool Equals(CoreIdentifier? other)
	{
		return this.coreId.Equals(other);
	}

	public override string ToString()
	{
		return $"{"Id".Localize()}: {this.TagId}, {"Position".Localize()}: {this.Position}";
	}
}
