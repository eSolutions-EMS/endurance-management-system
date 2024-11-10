using Newtonsoft.Json;
using NTS.Domain.Setup.Import;
using static NTS.Domain.Enums.OfficialRole;

namespace NTS.Domain.Setup.Entities;

public class Official : DomainEntity, ISummarizable, IImportable
{
    public static Official Create(string? names, OfficialRole? role)
    {
        return new(Person.Create(names), role);
    }

    public static Official Update(int id, string? names, OfficialRole? role)
    {
        return new(id, Person.Create(names), role);
    }

    [JsonConstructor]
    private Official(int id, Person? person, OfficialRole? role)
        : base(id)
    {
        var name = person;
        Role = Required(nameof(Role), role);
        Person = Required(nameof(Person), person);
    }
    private Official(Person? person, OfficialRole? role)
        : this(GenerateId(), person, role) { }

    public Person Person { get; }
    public OfficialRole Role { get; }

    public override string ToString()
    {
        return Combine(Role.GetDescription(), Person);
    }

    public bool IsUniqueRole()
    {
        return Role
            is VeterinaryCommissionPresident
                or GroundJuryPresident
                or ForeignVeterinaryDelegate
                or TechnicalDelegate
                or ForeignJudge;
    }
}
