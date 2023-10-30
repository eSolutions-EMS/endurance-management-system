namespace Common.Application.CRUD;

public interface ICreate<T>
{
    T CreateModel { get; }
    Task Create();
}
