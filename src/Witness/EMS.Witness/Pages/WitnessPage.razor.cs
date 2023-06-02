using Core.Domain.AggregateRoots.Manager;
using EMS.Witness.Rpc;
using EMS.Witness.Services;
using Microsoft.AspNetCore.Components;

namespace EMS.Witness.Pages;

public partial class WitnessPage : ComponentBase
{
    [Inject]
    private State State { get; set; } = null!;
    private Model witnessModel = new();

    protected override void OnInitialized()
    {
        this.witnessModel.Type = this.State.Type.ToString();
    }

    private void SetType(ChangeEventArgs args)
    {
        var value = (WitnessEventType)args.Value!;
        this.State.Type = value;
    }

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

    private class Model
    {
        public string Type;
        public int? Number;
        public int? Hour;
        public int? Minute;
        public int? Second;
        public int? Millisecond;
    }
}
