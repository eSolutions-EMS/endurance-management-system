using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Configuration.Extensions;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Phases;
using System;
using System.Linq;
using static EnduranceJudge.Localization.Translations.Words;
using static EnduranceJudge.Localization.Translations.Messages;

namespace EnduranceJudge.Domain.Aggregates.Configuration
{
    public class PhasesManager : ManagerObjectBase
    {
        private readonly IState state;
        private readonly Validator<PhaseException> validator;

        internal PhasesManager(IState state)
        {
            this.state = state;
            this.validator = new Validator<PhaseException>();
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

            this.Validate(phaseState, competitionId);

            phase = new Phase(phaseState);
            competition.Save(phase);

            return phase;
        }

        public Phase Update(IPhaseState phaseState)
        {
            this.state.ValidateThatEventHasNotStarted();

            foreach (var competition in this.state.Event.Competitions)
            {
                var phase = competition.Phases.FindDomain(phaseState.Id);
                if (phase == null)
                {
                    continue;
                }
                this.Validate(phaseState, competition.Id);

                phase.IsFinal = phaseState.IsFinal;
                phase.OrderBy = phaseState.OrderBy;
                phase.LengthInKm = phaseState.LengthInKm;
                phase.MaxRecoveryTimeInMins = phaseState.MaxRecoveryTimeInMins;
                phase.RestTimeInMins = phaseState.RestTimeInMins;
                phase.IsCompulsoryInspectionRequired = phaseState.IsCompulsoryInspectionRequired;
                this.UpdateParticipations(phase);
                return phase;
            }

            throw new InvalidOperationException($"Phase with Id '{phaseState.Id}' does not exist");
        }

        private void UpdateParticipations(Phase phase)
        {
            foreach (var participation in this.state.Participants.Select(x => x.Participation))
            {
                foreach (var competition in participation.Competitions)
                {
                    if (competition.Phases.Contains(phase))
                    {
                        competition.Phases
                            .FindDomain(phase.Id)
                            .MapFrom(phase);
                    }
                }
            }
        }
        private void Validate(IPhaseState phaseState, int competitionId)
        {
            var competition = this.state.Event.Competitions.FindDomain(competitionId);
            if (competition.Phases.Any(x => x.OrderBy == phaseState.OrderBy && x.Id != phaseState.Id))
            {
                throw DomainExceptionBase.Create<PhaseException>(INVALID_ORDER_BY, phaseState.OrderBy);
            }
            if (!phaseState.IsFinal)
            {
                this.validator.IsRequired(phaseState.RestTimeInMins, REST_TIME_IN_MINS);
            }
            this.validator.IsRequired(phaseState.OrderBy, ORDER);
            this.validator.IsRequired(phaseState.LengthInKm, LENGTH_IN_KM);
            this.validator.IsRequired(phaseState.MaxRecoveryTimeInMins, RECOVERY_IN_MINUTES_TEXT);
        }
    }
}
