using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Abstractions;

public abstract class EmsDomainBase<TException>
    : IEmsDomain,
        IEquatable<EmsDomainBase<TException>>,
        IEmsIdentifiable
    where TException : EmsDomainExceptionBase, new()
{
    protected const string GENERATE_ID = "GenerateIdFlag";

    // Empty constructor is used by mapping for existing (in the database) entries
    protected EmsDomainBase() { }

    // Unused variable is needed mark the constructor which generates Id
    // That constructor should ONLY be used when creating NEW (no database entry) objects
    protected EmsDomainBase(string _)
    {
        this.Id = EmsDomainIdProvider.Generate();
    }

    public int Id { get; protected init; } // Keep setter for mapping

    public override bool Equals(object? other) => this.IsEqual(other);

    public bool Equals(IEmsIdentifiable identifiable) => this.IsEqual(identifiable);

    public bool Equals(EmsDomainBase<TException>? domain) => this.IsEqual(domain);

    public static bool operator ==(EmsDomainBase<TException>? one, EmsDomainBase<TException>? two)
    {
        return one is null ? two is null : one.Equals(two);
    }

    public static bool operator !=(
        EmsDomainBase<TException>? one,
        EmsDomainBase<TException>? two
    ) => !(one == two);

    private bool IsEqual(object? other)
    {
        if (other is not IEmsIdentifiable identifiable)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (this.Id == identifiable.Id)
        {
            return true;
        }
        if (this.GetType() != other.GetType())
        {
            return false;
        }
        return this.GetHashCode().Equals(other.GetHashCode());
    }

    public override int GetHashCode() => this.Id;

    protected EmsValidator<TException> Validator { get; } = new();
}
