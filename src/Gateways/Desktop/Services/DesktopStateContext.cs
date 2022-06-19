using EnduranceJudge.Application;
using EnduranceJudge.Application.Services;
using EnduranceJudge.Domain.State;
using System;

namespace EnduranceJudge.Gateways.Desktop.Services;

// TODO: better name
public class DesktopStateContext : IStateContext
{
    private readonly State state = new();
    
    public DesktopStateContext(IDataService dataService)
    {
        var existingState = dataService.Get();
        if (existingState != null)
        {
            FixDatesForToday(existingState); // TODO: configuration and check
            this.state.Event = existingState.Event;
            this.state.Horses = existingState.Horses;
            this.state.Athletes = existingState.Athletes;
            this.state.Participants = existingState.Participants;
            this.state.Participations = existingState.Participations;
        }
    }

    public IState State => this.state;
    
    private void FixDatesForToday(State state)
    {
        if (state == null)
        {
            return;
        }
        if (state.Event != null)
        {
            foreach (var competition in state.Event.Competitions)
            {
                competition.StartTime = FixDateForToday(competition.StartTime);
            }
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
