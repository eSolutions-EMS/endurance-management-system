using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Domain.States
{
    public interface IPersonnelState : IDomainModelState
    {
        public string Name { get; }

        public PersonnelRole Role { get; }
    }
}
