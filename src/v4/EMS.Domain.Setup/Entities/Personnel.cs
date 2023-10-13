using EMS.Domain.Events.Start;
using EMS.Domain.Setup.Import;

namespace EMS.Domain.Setup.Entities;

public class Personnel : DomainEntity, ISummarizable, IImportable
{
    public Personnel(string name, PersonnelType type)
    {
		this.Person = new Person(name);
		this.Type = type;
    }

    public Person Person { get; private set; }
	public PersonnelType Type { get; private set; }

	public override string ToString()
	{
		return $"{Localizer.Get(this.Type)}: {this.Person}";
	}
}

public enum PersonnelType
{
	Steward = 1,
	MemberVet = 2,
	MemberJudge = 3,
	PresidentVet = 4,
	PresidentGroundJury = 5,
	ActiveVet = 6,
	ForeignJudge = 7,
	FeiDelegateVet = 8,
	FevDelegateTech = 9
}
