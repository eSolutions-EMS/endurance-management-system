using EMS.Domain.Setup.Import;

namespace EMS.Domain.Setup.Entities;

public class Official : DomainEntity, ISummarizable, IImportable
{
    public Official(int id, string name, OfficialRole role) : this(name, role)
    {
        this.Id = id;
    }
    public Official(string name, OfficialRole type)
    {
		this.Person = new Person(name);
		this.Role = type;
    }

    public Person Person { get; private set; }
	public OfficialRole Role { get; private set; }

	public override string ToString()
	{
		return $"{Localizer.Get(this.Role)}: {this.Person}";
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
