using EnduranceJudge.Domain.AggregateRoots.Configuration.Extensions;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Validation;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Laps;
using System;
using System.Linq;
using static EnduranceJudge.Domain.DomainConstants.ErrorMessages;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Domain.AggregateRoots.Configuration.Aggregates;

public class LapsAggregate : IAggregate
{
    private readonly IState state;
    private readonly Validator<LapException> validator;

    internal LapsAggregate(IState state)
    {
        this.state = state;
        this.validator = new Validator<LapException>();
    }

    public Lap Create(int competitionId, ILapState lapState)
    {
        this.state.ValidateThatEventHasNotStarted();

        var competition = this.state.Event.Competitions.FindDomain(competitionId);
        if (competition == null)
        {
            var message = string.Format(CANNOT_CREATE_PHASE_COMPETITION_DOES_NOT_EXIST, competitionId);
            throw new Exception(message);
        }
        var lap = competition.Laps.FindDomain(lapState.Id);
        if (lap != null)
        {
            var message = string.Format(CANNOT_CREATE_PHASE_IT_ALREADY_EXISTS, lapState.Id);
            throw new Exception(message);
        }

        this.Validate(lapState, competitionId);

        lap = new Lap(lapState);
        competition.Save(lap);

        return lap;
    }

    public Lap Update(ILapState lapState)
    {
        this.state.ValidateThatEventHasNotStarted();

        foreach (var competition in this.state.Event.Competitions)
        {
            var lap = competition.Laps.FindDomain(lapState.Id);
            if (lap == null)
            {
                continue;
            }
            this.Validate(lapState, competition.Id);

            lap.IsFinal = lapState.IsFinal;
            lap.OrderBy = lapState.OrderBy;
            lap.LengthInKm = lapState.LengthInKm;
            lap.MaxRecoveryTimeInMins = lapState.MaxRecoveryTimeInMins;
            lap.RestTimeInMins = lapState.RestTimeInMins;
            lap.IsCompulsoryInspectionRequired = lapState.IsCompulsoryInspectionRequired;
            return lap;
        }

        var message = string.Format(CANNOT_UPDATE_PHASE_IT_DOES_NOT_EXIST, lapState.Id);
        throw new InvalidOperationException(message);
    }

    private void Validate(ILapState lapState, int competitionId)
    {
        var competition = this.state.Event.Competitions.FindDomain(competitionId);
        var otherLaps = competition.Laps
            .Where(x => x.Id != lapState.Id)
            .ToList();
        if (lapState.IsFinal)
        {
            if (otherLaps.Any(x => x.IsFinal))
            {
                throw Helper.Create<LapException>(FINAL_LAP_ALREADY_EXISTS_MESSAGE);
            }
            if (otherLaps.Any(x => x.OrderBy >= lapState.OrderBy))
            {
                throw Helper.Create<LapException>(FINAL_LAP_MUST_HAVE_HIGHEST_ORDER_MESSAGE);
            }
        }
        else
        {
            var final = otherLaps.FirstOrDefault(x => x.IsFinal);
            if (final is not null)
            {
                throw Helper.Create<LapException>(LAP_MUST_HAVE_LOWER_ORDER_THAN_FINAL_LAP_MESSAGE);
            }
            this.validator.IsRequired(lapState.RestTimeInMins, REST_TIME_IN_MINS);
        }
        if (competition.Laps.Any(x => x.OrderBy == lapState.OrderBy && x.Id != lapState.Id))
        {
            throw Helper.Create<LapException>(INVALID_ORDER_BY_MESSAGE, lapState.OrderBy);
        }
        this.validator.IsRequired(lapState.OrderBy, ORDER);
        this.validator.IsRequired(lapState.LengthInKm, LENGTH_IN_KM);
        this.validator.IsRequired(lapState.MaxRecoveryTimeInMins, RECOVERY_IN_MINUTES_TEXT);
    }
}
