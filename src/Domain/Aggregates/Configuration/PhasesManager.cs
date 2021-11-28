using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Phases;
using System;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class PhasesManager : ManagerObjectBase
    {
        private readonly IState state;

        internal PhasesManager(IState state)
        {
            this.state = state;
        }

        public Phase Create(int competitionId, IPhaseState state)
        {
            var competition = this.state.Event.Competitions.FindDomain(competitionId);
            if (competition == null)
            {
                var message = $"Cannot save Phase - competition with id '{competitionId}' does not exit";
                throw new InvalidOperationException(message);
            }
            var phase = competition.Phases.FindDomain(state.Id);
            if (phase != null)
            {
                var message = $"Cannot create phase. A phase with Id '{state.Id}' already exists";
                throw new InvalidOperationException(message);
            }

            this.ValidateState(state);

            phase = new Phase(state);
            competition.Save(phase);

            return phase;
        }

        public Phase Update(IPhaseState state)
        {
            this.ValidateState(state);

            foreach (var competition in this.state.Event.Competitions)
            {
                var phase = competition.Phases.FindDomain(state.Id);
                if (phase == null)
                {
                    continue;
                }

                phase.IsFinal = state.IsFinal;
                phase.OrderBy = state.OrderBy;
                phase.LengthInKm = state.LengthInKm;
                phase.MaxRecoveryTimeInMins = state.MaxRecoveryTimeInMins;
                phase.RestTimeInMins = state.RestTimeInMins;
                return phase;
            }

            throw new InvalidOperationException($"Phase with Id '{state.Id}' does not exist");
        }

        private void ValidateState(IPhaseState state)
        {
            this.Validate<PhaseException>(() =>
            {
                if (!state.IsFinal)
                {
                    state.RestTimeInMins.IsRequired(REST_TIME_IN_MINS);
                }
                state.OrderBy.IsRequired(ORDER);
                state.LengthInKm.IsRequired(LENGTH_IN_KM);
                state.MaxRecoveryTimeInMins.IsRequired(RECOVERY_IN_MINUTES_TEXT);
            });
        }
    }
}
