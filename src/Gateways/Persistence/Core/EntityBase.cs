using EnduranceJudge.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnduranceJudge.Gateways.Persistence.Core
{
    public abstract class EntityBase : ObjectBase
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        [JsonIgnore]
        public abstract IEnumerable<Type> DomainTypes { get; }
    }
}
