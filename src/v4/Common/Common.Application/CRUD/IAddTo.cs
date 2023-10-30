using Common.Conventions;

namespace Common.Application.CRUD;

public interface IAdd<T>
{
    Task Add(T model);
}
