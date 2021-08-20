using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Domain.Aggregates.Event.Personnels
{
    public interface IPersonnelState : IDomainModelState
    {
        public string Name { get; }

        public PersonnelRole Role { get; }
    }
}
