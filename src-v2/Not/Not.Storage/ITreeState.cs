namespace Not.Storage;

public interface ITreeState<T>
    where T : DomainEntity
{
    T? Root { get; set; }
}
