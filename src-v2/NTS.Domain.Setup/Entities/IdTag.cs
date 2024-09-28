namespace NTS.Domain.Setup.Entities;

public class IdTag : DomainEntity, ISummarizable
{
    //remove when integrating controllers
    //public IdTag(string tagData)
    //{
    //    var number = tagData.Substring(0, 3);
    //    var id = tagData.Substring(9);
    //    Id = int.Parse(id);
    //    Number = int.Parse(number);
    //}
    public IdTag(string tagId, int number)
    {
		TagId = tagId;
        Number = number;
    }

    public string TagId { get; private set; }
    public int Number { get; }

    public override string ToString()
	{
        return Combine($"{"id".Localize()}: {TagId}");
    }
}
