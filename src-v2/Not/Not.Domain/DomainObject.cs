namespace Not.Domain;

public abstract record DomainObject
{
    protected string Combine(params object?[] values)
    {
        var sections = values.Where(x => x != null);
        return string.Join(" | ", sections);
    }
}
