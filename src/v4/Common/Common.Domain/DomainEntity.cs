using Common.Utilities;

namespace Core.Domain.Common.Models;

public abstract class DomainEntity: IEquatable<DomainEntity>
{
    protected const string GENERATE_ID = "GenerateIdFlag";
    
    protected DomainEntity()
    {
        this.Id = RandomUtilities.GenerateUniqueInteger();
    }

    public int Id { get; protected init; }

    public bool Equals(DomainEntity? other)
        => this.IsEqual(other);
    public override bool Equals(object? other)
        => this.IsEqual(other);

    public static bool operator ==(DomainEntity one, DomainEntity other)
    {
        if (ReferenceEquals(one, null))
        {
            return ReferenceEquals(other, null);
        }
        return one.Equals(other);
    }

    public static bool operator !=(DomainEntity one, DomainEntity other)
        => !(one == other);

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
