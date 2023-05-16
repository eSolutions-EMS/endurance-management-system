using EMS.Core.Domain.Core.Exceptions;
using EMS.Core.Domain.Core.Models;
using EMS.Core.Domain.State;
using EMS.Core.Domain.State.Competitions;
using EMS.Core.Domain.State.Participants;
using EMS.Core.Domain.Validation;
using EMS.Core.Domain.AggregateRoots.Configuration.Extensions;
using EMS.Core.Domain.Core.Extensions;
using System.Linq;
using static EMS.Core.Localization.Strings;

namespace EMS.Core.Domain.AggregateRoots.Configuration.Aggregates;

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
