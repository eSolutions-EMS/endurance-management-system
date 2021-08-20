using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.Aggregates.Event.Personnels;

namespace EnduranceJudge.Application.Events.Factories.Implementations
{
    public class PersonnelFactory : IPersonnelFactory
    {
        public Personnel Create(PersonnelDependantModel data)
        {
            var personnel = new Personnel(data.Id, data.Name, data.Role);
            return personnel;
        }
    }
}
