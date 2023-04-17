using Endurance.Gateways.Witness.Models;
using Endurance.Gateways.Witness.Services;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using Microsoft.AspNetCore.Components;

namespace Endurance.Gateways.Witness.Pages;
public partial class WitnessPage : ComponentBase
{ 
    [Inject]
    private IApiService ApiService { get; set; }

    private Model witnessModel = new();
    private Dictionary<int, ManualWitnessEvent> recordedEvents = new();

    private void HandleSubmit()
    {
        var witnessEvent = new ManualWitnessEvent
        {
            Number = this.witnessModel.Number.Value,
            Time = DateTime.Today
                .AddHours(this.witnessModel.Hour.Value)
                .AddMinutes(this.witnessModel.Minute.Value)
                .AddSeconds(this.witnessModel.Second.Value),
            Type = Enum.Parse<WitnessEventType>(this.witnessModel.Type),
        };
        if (this.recordedEvents.ContainsKey(witnessEvent.Number))
        {
            this.recordedEvents.Remove(witnessEvent.Number);
        }
        this.recordedEvents.Add(witnessEvent.Number, witnessEvent);
    }

    private async Task Save(int number)
    {
        var witnessEvent = this.recordedEvents[number];
        var isSuccess = await this.ApiService.PostWitnessEvent(witnessEvent);
        if (isSuccess)
        {
			this.recordedEvents.Remove(number);
        }
        
    }

    private class Model
    {
        public string Type = WitnessEventType.Arrival.ToString();
        public int? Number;
        public int? Hour;
        public int? Minute;
        public int? Second;
    }
}


