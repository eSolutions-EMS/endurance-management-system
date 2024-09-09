namespace NTS.Domain.Setup.Entities;

public class IdTag : DomainEntity, ISummarizable
{
    public IdTag(string tagId, string position, int number)
    {
		TagId = tagId;
		Position = position;
        Number = number;
    }

    public string TagId { get; private set; }
	public string Position { get; private set; }
    public int Number { get; }

    public override string ToString()
	{
		return $"{"Id".Localize()}: {this.TagId}, {"Position".Localize()}: {this.Position}";
	}
}
