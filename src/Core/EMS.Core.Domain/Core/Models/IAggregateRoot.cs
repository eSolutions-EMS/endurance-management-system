using EMS.Core.ConventionalServices;

namespace EMS.Core.Domain.Core.Models;

// TODO: Make Singleton
public interface IAggregateRoot : IAggregate, ITransientService
{
}
