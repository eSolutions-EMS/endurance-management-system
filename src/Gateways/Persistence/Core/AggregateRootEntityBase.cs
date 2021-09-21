using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnduranceJudge.Gateways.Persistence.Core
{
    public abstract class AggregateRootEntityBase : EntityBase
    {
        [NotMapped]
        [JsonIgnore]
        public abstract IEnumerable<Type> DomainTypes { get; }
    }
}
