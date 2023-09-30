using Common.Utilities;

namespace Core.Domain.Common.Models;

public abstract class DomainEntity: IEquatable<DomainEntity>
{
    protected DomainEntity()
    {
        this.Id = RandomUtilities.GenerateUniqueInteger();
    }

    public int Id { get; protected init; }

    public bool Equals(DomainEntity? other)
        => this.IsEqual(other);
    public override bool Equals(object? other)
        => this.IsEqual(other);

    private bool IsEqual(object? other)
    {
        if (other == null || other is not DomainEntity domainEntity)
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
}
