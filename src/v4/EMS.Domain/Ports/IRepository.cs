namespace EMS.Domain.Ports;

public interface IRepository<T>
	where T : DomainEntity
{
	Task Create(T entity);
	Task Read(int id);
	Task Read(Predicate<T> filter);
	Task Delete(T entity);
	Task Delete(int id);
	Task Delete(Predicate<T> filter);
}
