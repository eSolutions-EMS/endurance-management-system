namespace Not.Storage;

public interface IFlatState<T>
    where T : DomainEntity
{
    T? Entity { get; set; }
}
