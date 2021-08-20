using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Domain.States
{
    public interface IAthleteState : IDomainModelState
    {
        public string FeiId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string CountryIsoCode { get; }
        public Category Category { get; }
    }
}
