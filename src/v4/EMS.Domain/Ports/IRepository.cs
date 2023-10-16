using Core.Models;

namespace EMS.Domain.Ports;

public interface IRepository<T>
	where T : DomainEntity
{
	Task<Result> Create(T entity);
	Task<Result<T>> Read(int id);
	Task<Result<IEnumerable<T>>> Read(Predicate<T> filter);
	Task<Result> Delete(T entity);
	Task<Result> Delete(int id);
	Task<Result<int>> Delete(Predicate<T> filter);
}
