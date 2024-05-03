namespace Not.Storage;

public interface ISetState<T>
    where T : DomainEntity
{
    List<T> EntitySet { get; }
}
