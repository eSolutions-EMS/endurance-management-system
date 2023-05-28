using Core.Application.Services;
using Core.Domain.AggregateRoots.Manager;
using EMS.Witness.Rpc;
using EMS.Witness.Services;
using Microsoft.AspNetCore.Components;

namespace EMS.Witness.Pages;

public partial class WitnessPage : ComponentBase
{
    [Inject] 
    private IDateService DateService { get; set; } = null!;
    [Inject]
    private IState State { get; set; } = null!;
    [Inject]
    private IWitnessEventClient WitnessEventClient { get; set; } = default!;

    private Model witnessModel = new();

    private void HandleSubmit()
    {
        var now = DateTime.Now;
        var witnessEvent = new WitnessEvent
        {
            TagId = this.witnessModel.Number!.Value.ToString(),
            Time = DateTime.Today
                .AddHours(this.witnessModel.Hour ?? now.Hour)
                .AddMinutes(this.witnessModel.Minute ?? now.Minute)
                .AddSeconds(this.witnessModel.Second ?? now.Second)
                .AddMilliseconds(this.witnessModel.Millisecond ?? now.Millisecond),
            Type = Enum.Parse<WitnessEventType>(this.witnessModel.Type),
        };
        if (this.State.WitnessRecords.ContainsKey(witnessEvent.TagId))
        {
            this.State.WitnessRecords.Remove(witnessEvent.TagId);
        }
        this.State.WitnessRecords.Add(witnessEvent.TagId, witnessEvent);
    }

    private async Task Save(string number)
    {
		var witnessEvent = this.State.WitnessRecords[number];
		await this.WitnessEventClient.Add(witnessEvent);
		this.State.WitnessRecords.Remove(number);
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
