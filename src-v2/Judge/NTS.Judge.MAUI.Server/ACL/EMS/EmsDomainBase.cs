using Not;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public abstract class EmsDomainBase<TException> : IIdentifiable, IEquatable<EmsDomainBase<TException>>
    where TException : EmsDomainExceptionBase, new()
{
    protected const string GENERATE_ID = "GenerateIdFlag";

    // Empty constructor is used by mapping for existing (in the database) entries
    protected EmsDomainBase()
    {
    }
    // Unused variable is needed mark the constructor which generates Id
    // That constructor should ONLY be used when creating NEW (no database entry) objects
    protected EmsDomainBase(int id)
    {
        Id = id;
    }

    public int Id { get; protected init; } // Keep setter for mapping

    public override bool Equals(object other)
        => IsEqual(other);

    public bool Equals(IIdentifiable identifiable)
        => IsEqual(identifiable);

    public bool Equals(EmsDomainBase<TException> domain)
        => IsEqual(domain);

    public static bool operator ==(EmsDomainBase<TException> one, EmsDomainBase<TException> two)
    {
        if (ReferenceEquals(one, null))
        {
            return ReferenceEquals(two, null);
        }
        return one.Equals(two);
    }

    public static bool operator !=(EmsDomainBase<TException> one, EmsDomainBase<TException> two)
        => !(one == two);

    private bool IsEqual(object other)
    {
        if (other is not IIdentifiable identifiable)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (Id == identifiable.Id)
        {
            return true;
        }
        if (GetType() != other.GetType())
        {
            return false;
        }

        return GetHashCode().Equals(other.GetHashCode());
    }

    public override int GetHashCode()
        => Id;

    protected EmsValidator<TException> Validator { get; } = new();
}
