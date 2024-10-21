namespace NTS.Domain.Setup.Entities;

public class Tag : DomainEntity, ISummarizable
{
    public Tag(string tagId, int number)
    {
		TagId = tagId;
        Number = number;
    }

    public string TagId { get; }
    public int Number { get; }

    public override string ToString()
	{
        return Combine($"{"Tag Id".Localize()}: {TagId}");
    }
}
