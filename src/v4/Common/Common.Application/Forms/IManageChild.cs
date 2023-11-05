using Common.Conventions;
using Common.Domain;

namespace Common.Application.Forms;

public interface IManageChild<T, TChild> : ISingletonService
    where T : DomainEntity, IParent<TChild>
    where TChild : DomainEntity
{
    T? Entity { get; }
    Task Add(TChild child);
    Task Remove(TChild child);
    Task Update(TChild child);
}
