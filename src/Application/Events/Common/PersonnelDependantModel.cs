using EnduranceJudge.Domain.Aggregates.Event.Personnels;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Application.Events.Common
{
    public class PersonnelDependantModel : IPersonnelState
    {
        public int Id { get; set; }

        public PersonnelRole Role { get; set; }

        public string Name { get; set; }
    }
}
