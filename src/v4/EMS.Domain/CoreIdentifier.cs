namespace EMS.Domain;

public abstract class CoreIdentifier : ICoreIdentifier
{
    public int Number { get; protected set; }
    public bool Equals(ICoreIdentifier? other)
    {
        return this.Number == other?.Number;
    }
}

public interface ICoreIdentifier : IEquatable<ICoreIdentifier>
{
    public int Number { get; }
}
