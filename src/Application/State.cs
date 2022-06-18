using EnduranceJudge.Application.Services;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Application;

public class State : IState
{
    // public State()
    // {
        // var dataService = StaticProvider.GetService<IDataService>();
        // var existingState = dataService.Get();
        // if (existingState != null)
        // {
        //     FixDatesForToday(existingState); // TODO: configuration and check
        //     this.Event = existingState.Event;
        //     this.Horses = existingState.Horses;
        //     this.Athletes = existingState.Athletes;
        //     this.Participants = existingState.Participants;
        //     this.Participations = existingState.Participations;
        // }
    // }
    
    public EnduranceEvent Event { get; set; }
    public List<Horse> Horses { get; private set; } = new();
    public List<Athlete> Athletes { get; private set; } = new();
    public List<Participant> Participants { get; private set; } = new();
    public List<Participation> Participations { get; private set; } = new();

    [JsonIgnore]
    public IReadOnlyList<Country> Countries
        => ApplicationConstants.Countries.List.AsReadOnly();
    
    private void FixDatesForToday(State state)
    {
        if (state == null)
        {
            return;
        }
        foreach (var competition in state.Event.Competitions)
        {
            competition.StartTime = FixDateForToday(competition.StartTime);
        }
        foreach (var participant in state.Participants)
        {
            foreach (var performance in participant.LapRecords)
            {
                performance.StartTime = FixDateForToday(performance.StartTime);
                if (performance.ArrivalTime.HasValue)
                {
                    performance.ArrivalTime = FixDateForToday(performance.ArrivalTime.Value);
                }
                if (performance.InspectionTime.HasValue)
                {
                    performance.InspectionTime = FixDateForToday(performance.InspectionTime.Value);
                }
                if (performance.ReInspectionTime.HasValue)
                {
                    performance.ReInspectionTime = FixDateForToday(performance.ReInspectionTime.Value);
                }
            }
        }
    }

    private DateTime FixDateForToday(DateTime date)
    {
        var today = DateTime.Today;
        today = today.AddHours(date.Hour);
        today = today.AddMinutes(date.Minute);
        today = today.AddSeconds(date.Second);
        today = today.AddMilliseconds(date.Millisecond);
        return today;
    }
}
