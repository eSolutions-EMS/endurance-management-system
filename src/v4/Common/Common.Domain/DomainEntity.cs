using Common.Utilities;

namespace Common.Domain;

public abstract class DomainEntity : IEquatable<DomainEntity>, IIdentifiable
{
    protected DomainEntity()
    {
        this.Id = RandomHelper.GenerateUniqueInteger();
    }

    public int Id { get; protected init; }

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
