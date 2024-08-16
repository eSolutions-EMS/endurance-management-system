namespace NTS.Domain.Core.Entities;

public class Official : DomainEntity
{
    public Official(Person person, OfficialRole role)
    {
        Person = person;
        Role = role;
    }

    public Person Person { get; private set; }
    public OfficialRole Role { get; private set; }

    public override string ToString()
    {
        return $"{Role}: {Person}";
    }
}
