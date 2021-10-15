using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Phases;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class PhasesManager : ManagerObjectBase
    {
        private readonly IState state;

        internal PhasesManager(IState state)
        {
            this.state = state;
        }

        public void Create(int competitionId, IPhaseState state)
        {
            var phase = new Phase(state);
            var competition = this.state.Event.Competitions.FindDomain(competitionId);
            competition.Save(phase);
        }

        public void Update(IPhaseState state)
        {
            var phase = new Phase(state);
            foreach (var competition in this.state.Event.Competitions)
            {
                if (competition.Phases.AnyMatch(phase))
                {
                    competition.Save(phase);
                }
            }
        }
    }
}
