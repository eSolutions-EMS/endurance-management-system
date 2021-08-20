using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.Aggregates.Event.Personnels;

namespace EnduranceJudge.Application.Events.Factories
{
    public interface IPersonnelFactory : IService
    {
        Personnel Create(PersonnelDependantModel data);
    }
}
