namespace Not.Domain;

public abstract record DomainObject
{
    protected string Combine(params object?[] values)
    {
        return string.Join(" | ", values.Where(x => x != null));
    }
}