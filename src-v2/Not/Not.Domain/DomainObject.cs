using Not.Extensions;

namespace Not.Domain;

// TODO: use same validation as in DomainEntity. Maybe drop DomainObject being a record entirely?
public abstract record DomainObject
{
    protected string Combine(params object?[] values)
    {
        return DomainModelHelper.Combine(values);
    }
}
