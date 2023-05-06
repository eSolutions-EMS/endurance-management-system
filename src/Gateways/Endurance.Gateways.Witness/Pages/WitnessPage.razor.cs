using Endurance.Gateways.Witness.Models;
using Endurance.Gateways.Witness.Services;
using EnduranceJudge.Application.Services;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using Microsoft.AspNetCore.Components;

namespace Endurance.Gateways.Witness.Pages;
public partial class WitnessPage : ComponentBase
{
    [Inject] private IDateService DateService { get; set; } = null!;
    [Inject] private IApiService ApiService { get; set; } = null!;
    [Inject] private IState State { get; set; } = null!;

    private Model witnessModel = new();

    private void HandleSubmit()
    {
        var now = DateTime.Now;
        var witnessEvent = new ManualWitnessEvent
        {
            Number = this.witnessModel.Number!.Value,
            Time = DateTime.Today
                .AddHours(this.witnessModel.Hour ?? now.Hour)
                .AddMinutes(this.witnessModel.Minute ?? now.Minute)
                .AddSeconds(this.witnessModel.Second ?? now.Second)
                .AddMilliseconds(this.witnessModel.Millisecond ?? now.Millisecond),
            Type = Enum.Parse<WitnessEventType>(this.witnessModel.Type),
        };
        if (this.State.WitnessRecords.ContainsKey(witnessEvent.Number))
        {
            this.State.WitnessRecords.Remove(witnessEvent.Number);
        }
        this.State.WitnessRecords.Add(witnessEvent.Number, witnessEvent);
    }

    private async Task Save(int number)
    {
        var witnessEvent = this.State.WitnessRecords[number];
        var isSuccess = await this.ApiService.PostWitnessEvent(witnessEvent);
        if (isSuccess)
        {
			this.State.WitnessRecords.Remove(number);
        }
    }

    private class Model
    {
        public string Type = WitnessEventType.Arrival.ToString();
        public int? Number;
        public int? Hour;
        public int? Minute;
        public int? Second;
        public int? Millisecond;
    }
}


