using Core.ConventionalServices;

namespace Core.Domain.Common.Models;

// TODO: Make Singleton
public interface IAggregateRoot : IAggregate, ITransientService
{
}
