using EnduranceJudge.Domain.Aggregates.Configuration.Extensions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Phases;
using System;
using static EnduranceJudge.Localization.Translations.Words;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class PhasesManager : ManagerObjectBase
    {
        private readonly IState state;

        internal PhasesManager(IState state)
        {
            this.state = state;
        }

        public Phase Create(int competitionId, IPhaseState phaseState)
        {
            this.state.ValidateThatEventHasNotStarted();

            var competition = this.state.Event.Competitions.FindDomain(competitionId);
            if (competition == null)
            {
                var message = $"Cannot save Phase - competition with id '{competitionId}' does not exit";
                throw new InvalidOperationException(message);
            }
            var phase = competition.Phases.FindDomain(phaseState.Id);
            if (phase != null)
            {
                var message = $"Cannot create phase. A phase with Id '{phaseState.Id}' already exists";
                throw new InvalidOperationException(message);
            }

            this.Validate(phaseState);

            phase = new Phase(phaseState);
            competition.Save(phase);

            return phase;
        }

        public Phase Update(IPhaseState phaseState)
        {
            this.state.ValidateThatEventHasNotStarted();
            this.Validate(phaseState);

            foreach (var competition in this.state.Event.Competitions)
            {
                var phase = competition.Phases.FindDomain(phaseState.Id);
                if (phase == null)
                {
                    continue;
                }

                phase.IsFinal = phaseState.IsFinal;
                phase.OrderBy = phaseState.OrderBy;
                phase.LengthInKm = phaseState.LengthInKm;
                phase.MaxRecoveryTimeInMins = phaseState.MaxRecoveryTimeInMins;
                phase.RestTimeInMins = phaseState.RestTimeInMins;
                phase.RequireCompulsoryInspection = phaseState.RequireCompulsoryInspection;
                return phase;
            }

            throw new InvalidOperationException($"Phase with Id '{phaseState.Id}' does not exist");
        }

        private void Validate(IPhaseState state)
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
