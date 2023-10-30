namespace Common.Application.CRUD;

public interface IUpdate<T>
{
    T? UpdateModel { get; }
    Task Update();
}
