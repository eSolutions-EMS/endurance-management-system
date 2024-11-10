namespace NTS.Domain.Objects;

public record Club : DomainObject
{
    public static Club? Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }
        return new Club(name);
    }

    public Club(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public override string ToString()
    {
        return Name;
    }
}
