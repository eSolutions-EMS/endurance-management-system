using Not.Extensions;

namespace Not.Domain;

public abstract record DomainObject
{
    protected string Combine(params object?[] values)
    {
        return DomainModelHelper.Combine(values);
    }
}
