using Not.Random;

namespace Not.Domain;

public abstract class DomainEntity : IEquatable<DomainEntity>, IIdentifiable
{
    protected DomainEntity()
    {
        this.Id = RandomHelper.GenerateUniqueInteger();
    }

    // TODO: use DomainObject for ID
    public int Id { get; protected init; }

    public static bool operator == (DomainEntity? left, DomainEntity? right)
        => left?.IsEqual(right) ?? right is null;
    public static bool operator !=(DomainEntity? left, DomainEntity? right)
        => !(left == right);

    public bool Equals(DomainEntity? other)
        => this.IsEqual(other);
    public override bool Equals(object? other)
        => this.IsEqual(other);

    private bool IsEqual(object? other)
    {
        if (other == null || other is not DomainEntity)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return this.GetHashCode() == other.GetHashCode()
            && this.GetType() == other.GetType();
    }

    public override int GetHashCode()
        => this.Id;

	public override string ToString()
	{
        throw new NotImplementedException($"'{this.GetType().Name}' has to override ToString() to provide short info");
	}
}
