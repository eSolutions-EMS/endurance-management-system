using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents
{
    public interface IEnduranceEventState : IDomainModelState
    {
        public string Name { get; }

        public string PopulatedPlace { get; }

        public string CountryIsoCode { get; }
    }
}
