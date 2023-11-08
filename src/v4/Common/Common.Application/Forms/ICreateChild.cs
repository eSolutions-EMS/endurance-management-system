using Common.Conventions;
using Common.Domain;

namespace Common.Application.Forms;

public interface ICreateChild<T> : ISingletonService
    where T : DomainEntity
{
    Task Add(T child);
}
