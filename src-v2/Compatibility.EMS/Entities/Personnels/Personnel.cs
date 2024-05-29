using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Personnels;

public class Personnel : DomainBase<PersonnelException>, IPersonnelState
{
    private Personnel() {}
    public Personnel(IPersonnelState state) : base(GENERATE_ID)
    {
        this.Name = this.Validator.IsFullName(state.Name);
        this.Role = this.Validator.IsRequired(state.Role, nameof(state.Role));
    }

    public string Name { get; private set; }
    public PersonnelRole Role { get; private set; }
}

public enum PersonnelRole
{
    Invalid = 0,
    PresidentGroundJury = 1,
    PresidentVetCommission = 2,
    ForeignJudge = 3,
    FeiTechDelegate = 4,
    FeiVetDelegate = 5,
    ActiveVet = 6,
    MemberOfVetCommittee = 10,
    MemberOfJudgeCommittee = 11,
    Steward = 12,
}
