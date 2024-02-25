using Common.Conventions;
using Common.Domain;

namespace Common.Application.Behinds;

public interface IReadBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T> Read(int id);
}
