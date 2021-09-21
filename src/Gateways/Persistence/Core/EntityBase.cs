using EnduranceJudge.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace EnduranceJudge.Gateways.Persistence.Core
{
    public abstract class EntityBase : ObjectBase
    {
        [Key]
        public int Id { get; set; }
    }
}
