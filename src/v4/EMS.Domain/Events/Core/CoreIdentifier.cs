using Not.Domain;

namespace NTS.Domain;

public abstract record CoreIdentifier : DomainObject, ICoreIdentifier
{ 
    protected int Number { get; set; }

    public virtual bool Equals(CoreIdentifier? other)
    {
        return this.Number == other?.Number;
    }
    public override int GetHashCode()
        => this.Number.GetHashCode();
}

public interface ICoreIdentifier : IEquatable<CoreIdentifier>
{
}