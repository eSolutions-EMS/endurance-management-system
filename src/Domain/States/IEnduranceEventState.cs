using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.States
{
    public interface IEnduranceEventState : IDomainModelState
    {
        public string Name { get; }

        public string PopulatedPlace { get; }

        public string CountryIsoCode { get; }
    }
}
