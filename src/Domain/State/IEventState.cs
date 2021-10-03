using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.State
{
    public interface IEventState : IDomainModelState
    {
        public string Name { get; }
        public string PopulatedPlace { get; }
        public string CountryIsoCode { get; }
    }
}
