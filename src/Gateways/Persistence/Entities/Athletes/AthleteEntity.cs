using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.Countries;
using EnduranceJudge.Gateways.Persistence.Entities.Participants;
using Newtonsoft.Json;

namespace EnduranceJudge.Gateways.Persistence.Entities.Athletes
{
    public class AthleteEntity : EntityBase,
        IAthleteState
    {
        public string FeiId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public Category Category { get; set; }

        [JsonIgnore]
        public ParticipantEntity Participant { get; set; }
        public int ParticipantId { get; set; }

        [JsonIgnore]
        public CountryEntity Country { get; set; }
        public string CountryIsoCode { get; set; }
    }
}
