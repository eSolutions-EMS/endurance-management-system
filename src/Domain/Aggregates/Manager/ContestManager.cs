using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.Aggregates.Manager.Branches.StartList;
using EnduranceJudge.Domain.Aggregates.Manager.Participants;
using EnduranceJudge.Domain.Aggregates.Manager.Performances;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Localization.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Translations.Messages;

namespace EnduranceJudge.Domain.Aggregates.Manager;

public class ContestManager : ManagerObjectBase, IAggregateRoot
{
    private readonly IState state;

    public ContestManager()
    {
        this.state = StaticProvider.GetService<IState>();
        // TODO: think this is no longer necessary.
        // Check is necessary due to Prism's initialization logic which uses reflection
        // to generate instances of views as part of the startup process.
        // These views are not used in the actual views during the application use cycle
        if (this.state?.Event == null)
        {
            return;
        }
    }

    public bool HasStarted()
        => this.state.Participants.Any(x => x.Participation.Performances.Any());
    public void Start()
    {
        this.ValidateConfiguration();
        var participants = this.state
            .Participants
            .Select(x => new ParticipantManager(x))
            .ToList();
        foreach (var participant in participants)
        {
            participant.Start();
        }
        this.state.Event.HasStarted = true;
    }

    public void UpdatePerformance(int number, DateTime time)
    {
        var participant = this.GetParticipant(number);
        participant.UpdatePerformance(time);
    }
    public void CompletePerformance(int number, string code)
    {
        var participant = this.GetParticipant(number);
        var performance = participant.GetActivePerformance();
        performance.Complete(code);
    }
    public void ReInspection(int number, bool isRequired)
    {
        var participant = this.GetParticipant(number);
        var performance = participant.GetActivePerformance();
        performance.ReInspection(isRequired);
    }
    public void RequireInspection(int number, bool isRequired)
    {
        var participant = this.GetParticipant(number);
        var performance = participant.GetActivePerformance();
        performance.RequireInspection(isRequired);
    }
    public void EditPerformance(IPerformanceState state)
    {
        var performance = this.state
            .Participants
            .Select(part => part.Participation)
            .SelectMany(participant => participant.Performances)
            .FirstOrDefault(perf => perf.Equals(state));
        var manager = new PerformanceManager(performance);
        manager.Edit(state);
    }

    public IEnumerable<StartModel> GetStartList(bool includePast)
    {
        var startList = new StartList(this.state.Participants, includePast);
        return startList;
    }

    private ParticipantManager GetParticipant(int number)
    {
        var participant = this.state
            .Participants
            .FirstOrDefault(x => x.Number == number);
        if (participant == null)
        {
            var message = string.Format(Messages.PARTICIPANT_NUMBER_NOT_FOUND_TEMPLATE, number);
            throw new ParticipantException { DomainMessage = message };
        }
        var manager = new ParticipantManager(participant);
        return manager;
    }

    private void ValidateConfiguration()
    {
        foreach (var competition in this.state.Event.Competitions)
        {
            if (competition.Phases.All(x => !x.IsFinal))
            {
                throw new DomainException(INVALID_COMPETITION_NO_FINAL_PHASE, competition.Name);
            }
        }
        foreach (var participant in this.state.Participants)
        {
            if (!participant.Participation.Competitions.Any())
            {
                throw new DomainException(INVALID_PARTICIPANT_NO_PARTICIPATIONS, participant.Number);
            }
            if (participant.Athlete.Country == null)
            {
                throw new DomainException(INVALID_PARTICIPANT_NO_COUNTRY, participant.Number);
            }
        }
    }
}
