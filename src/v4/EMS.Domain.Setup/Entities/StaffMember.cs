using EMS.Domain.Events.Start;
using EMS.Domain.Setup.Import;

namespace EMS.Domain.Setup.Entities;

public class StaffMember : DomainEntity, ISummarizable, IImportable
{
    public StaffMember(string name, StaffRole type)
    {
		this.Person = new Person(name);
		this.Role = type;
    }

    public Person Person { get; private set; }
	public StaffRole Role { get; private set; }

	public override string ToString()
	{
		return $"{Localizer.Get(this.Role)}: {this.Person}";
	}
}

public enum StaffRole
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
