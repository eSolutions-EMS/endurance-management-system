using Not.Conventions;
using Not.Domain;

namespace Not.Application.Behinds;

public interface IReadBehind<T> : ISingletonService
    where T : DomainEntity
{
    Task<T> Read(int id);
}
