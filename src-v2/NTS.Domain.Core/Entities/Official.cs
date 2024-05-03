namespace NTS.Domain.Core.Entities;

public class Official : DomainEntity
{
    public Official(Person person, OfficialRole role)
    {
        Person = person;
        Role = role;
    }

    public Person Person { get; }
    public OfficialRole Role { get; }

    public override string ToString()
    {
        return $"{Person} {Role}";
    }
}
