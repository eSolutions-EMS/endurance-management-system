using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Horse : DomainEntity, ISummarizable, IImportable
{
    public static Horse Create(string name, string? feiId) => new (name, feiId);

    public static Horse Update(int id, string name, string? feiId) => new(id, name, feiId); 

    [JsonConstructor]
    private Horse(int id, string name, string? feiId) : this(name, feiId)
    {
        Id = id;
    }
    private Horse(string name, string? feiId)
    {
        if (name == null || name == "")
        {
            throw new DomainException(nameof(Name), "Name is required");
        }
        Name = name;
        FeiId = feiId;
    }

	public string? FeiId { get; private set; }
	public string Name { get; private set; }

	public string Summarize()
	{
        return ToString();
	}
	public override string ToString()
	{
		return Name;
	}
}
