using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Event.Personnels;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Gateways.Persistence.Core;
using EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Persistence.Entities.Personnels
{
    public class PersonnelEntity : EntityBase, IPersonnelState,
        IMap<Personnel>,
        IMapTo<PersonnelDependantModel>
    {
        private static readonly Type Domain = typeof(Personnel);

        public string Name { get; set; }
        public PersonnelRole Role { get; set; }

        [JsonIgnore]
        public EnduranceEventEntity EnduranceEvent { get; set; }
        public int EnduranceEventId { get; set; }

        public override IEnumerable<Type> DomainTypes { get; } = new[] { Domain };
    }
}
