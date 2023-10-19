using Common.Conventions;

namespace EMS.Domain.Ports;

public interface IRepository<T> : ISingletonService // Singleton for purposes ot simulation untill I get to implementing Persitence
	where T : DomainEntity
{
	Task<T> Create(T entity);
	Task<T?> Read(int id);
	Task<IEnumerable<T>> Read(Predicate<T> filter);
	Task<T> Update(T entity);
	Task Delete(T entity);
	Task Delete(int id);
	Task<int> Delete(Predicate<T> filter);
}
