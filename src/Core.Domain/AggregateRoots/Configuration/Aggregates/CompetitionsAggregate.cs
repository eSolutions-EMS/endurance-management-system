using Core.Domain.Core.Exceptions;
using Core.Domain.Core.Models;
using Core.Domain.State;
using Core.Domain.State.Competitions;
using Core.Domain.State.Participants;
using Core.Domain.Validation;
using Core.Domain.AggregateRoots.Configuration.Extensions;
using Core.Domain.Core.Extensions;
using System.Linq;
using static Core.Localization.Strings;

namespace Core.Domain.AggregateRoots.Configuration.Aggregates;

public class CompetitionsAggregate : IAggregate
{
    private readonly IState state;
    private readonly Validator<CompetitionException> validator;

    internal CompetitionsAggregate(IState state)
    {
        this.state = state;
        this.validator = new Validator<CompetitionException>();
    }

    public Competition Save(ICompetitionState competitionState)
    {
        this.state.ValidateThatEventHasNotStarted();
        this.validator.IsRequired(competitionState.Type, TYPE);
        this.validator.IsRequired(competitionState.Name, NAME);
        this.validator.IsRequired(competitionState.StartTime, START_TIME);

        var competition = this.state.Event.Competitions.FindDomain(competitionState.Id);
        if (competition == null)
        {
            competition = new Competition(competitionState);
            this.state.Event.Save(competition);
        }
        else
        {
            competition.Name = competitionState.Name;
            competition.Type = competitionState.Type;
            competition.StartTime = competitionState.StartTime;
        }

        return competition;
    }

    public void RemoveParticipation(int competitionId, int id)
    {
        this.state.ValidateThatEventHasNotStarted();
        var participation = this.state.Participations.FindDomain(id);
        if (participation == null)
        {
            throw Helper.Create<ParticipantException>(NOT_FOUND_BY_ID_MESSAGE);
        }
        participation.Remove(competitionId);
    }
}
