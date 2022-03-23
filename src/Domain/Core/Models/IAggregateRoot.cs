using EnduranceJudge.Core.ConventionalServices;

namespace EnduranceJudge.Domain.Core.Models;

public interface IAggregateRoot : IAggregate, ITransientService
{
}
