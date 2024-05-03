namespace Not.Storage.Ports.States;

public interface IFlatState<T>
    where T : DomainEntity
{
    T? Entity { get; set; }
}
