using EnduranceJudge.Domain.AggregateRoots.Configuration.Extensions;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Phases;
using System;
using System.Linq;
using static EnduranceJudge.Domain.DomainConstants.ErrorMessages;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Domain.AggregateRoots.Configuration.Aggregates
{
    public class PhasesAggregate : IAggregate
    {
        private readonly IState state;
        private readonly Validator<PhaseException> validator;

        internal PhasesAggregate(IState state)
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
                var message = string.Format(CANNOT_CREATE_PHASE_COMPETITION_DOES_NOT_EXIST, competitionId);
                throw new Exception(message);
            }
            var phase = competition.Phases.FindDomain(phaseState.Id);
            if (phase != null)
            {
                var message = string.Format(CANNOT_CREATE_PHASE_IT_ALREADY_EXISTS, phaseState.Id);
                throw new Exception(message);
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
                return phase;
            }

            var message = string.Format(CANNOT_UPDATE_PHASE_IT_DOES_NOT_EXIST, phaseState.Id);
            throw new InvalidOperationException(message);
        }

        private void Validate(IPhaseState phaseState, int competitionId)
        {
            var competition = this.state.Event.Competitions.FindDomain(competitionId);
            if (competition.Phases.Any(x => x.OrderBy == phaseState.OrderBy && x.Id != phaseState.Id))
            {
                throw Helper.Create<PhaseException>(INVALID_ORDER_BY_MESSAGE, phaseState.OrderBy);
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
