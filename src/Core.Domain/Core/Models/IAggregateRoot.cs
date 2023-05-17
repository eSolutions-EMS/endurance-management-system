using Core.ConventionalServices;

namespace Core.Domain.Core.Models;

// TODO: Make Singleton
public interface IAggregateRoot : IAggregate, ITransientService
{
}
