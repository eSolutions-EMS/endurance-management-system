using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Domain.State.Personnels
{
    public interface IPersonnelState : IIdentifiable
    {
        public string Name { get; }

        public PersonnelRole Role { get; }
    }
}
