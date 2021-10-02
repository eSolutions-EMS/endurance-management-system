using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.State.Personnels;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents;
using Newtonsoft.Json;

namespace EnduranceJudge.Gateways.Persistence.Entities.Personnels
{
    public class PersonnelEntity : EntityBase, IPersonnelState,
        IMap<Personnel>,
        IMapTo<PersonnelDependantModel>
    {
        public string Name { get; set; }
        public PersonnelRole Role { get; set; }

        [JsonIgnore]
        public EventEntity Event { get; set; }
        public int EnduranceEventId { get; set; }
    }
}
