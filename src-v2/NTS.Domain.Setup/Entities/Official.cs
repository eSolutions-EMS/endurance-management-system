using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Official : DomainEntity, ISummarizable, IImportable
{
    public static Official Create(string name, OfficialRole role) => new(new Person(name), role);
    public static Official Update(int id, string name, OfficialRole role) => new(id, new Person(name), role);

    [JsonConstructor]
    private Official(int id, Person person, OfficialRole role)
    {
        this.Person = person;
        this.Role = role;
        this.Id = id;
    }
    private Official(string name, OfficialRole role)
    {
        if (role == 0)
        {
            throw new DomainException(nameof(Role), "Official Role must have a value different from 0.");
        }

        this.Person = new Person(name);
		this.Role = role;
    }

    public Person Person { get; private set; }
	public OfficialRole Role { get; private set; }

	public override string ToString()
	{
        return $"{LocalizationHelper.Get(this.Role)}: {this.Person}";
	}
}

public enum OfficialRole
{
	Steward = 1,
	MemberVet = 2,
	MemberJudge = 3,
	PresidentVet = 4,
	PresidentGroundJury = 5,
	ActiveVet = 6,
	ForeignJudge = 7,
	FeiDelegateVet = 8,
	FeiDelegateTech = 9
}
