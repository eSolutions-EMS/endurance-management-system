using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;

namespace EnduranceJudge.Application.Events.Common
{
    public class PersonnelDependantModel : IPersonnelState
    {
        public int Id { get; set; }

        public PersonnelRole Role { get; set; }

        public string Name { get; set; }
    }
}
