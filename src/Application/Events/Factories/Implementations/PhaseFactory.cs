using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.Aggregates.Event.Phases;

namespace EnduranceJudge.Application.Events.Factories.Implementations
{
    public class PhaseFactory : IPhaseFactory
     {
         private readonly IPhaseForCategoryFactory phaseForCategoryFactory;

         public PhaseFactory(IPhaseForCategoryFactory phaseForCategoryFactory)
         {
             this.phaseForCategoryFactory = phaseForCategoryFactory;
         }

         public Phase Create(PhaseDependantModel data)
         {
             var phase = new Phase(data);
             foreach (var phaseData in data.PhasesForCategories)
             {
                var phaseForCategory = this.phaseForCategoryFactory.Create(phaseData);
                phase.Add(phaseForCategory);
             }

             return phase;
         }
     }
}
