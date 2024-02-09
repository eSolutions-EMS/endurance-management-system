using Common.Domain;

namespace Common.Application.CRUD;

public interface IRepository<T> : ICreate<T>, IRead<T>, IUpdate<T>, IDelete<T>
	where T : DomainEntity
{
	Task<IEnumerable<T>> Read(Predicate<T> filter);
	Task <T> Delete(int id);
	Task<T> Delete(Predicate<T> filter);
}
