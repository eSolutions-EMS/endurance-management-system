using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.WitnessEvents;
using EMS.Judge.Api.Requests;
using System;

namespace EMS.Judge.Api.Services;

public class WitnessEventService : IWitnessEventService
{
    public void AddEvent(WitnessEventType type, TagRequest request)
    {
        var time = this.GetSnapshotTime(request.Epoch);
        var witnessEvent = new WitnessEvent
        {
            Type = type,
            TagId = request.Id,
            Time = time,
        };
        Witness.Raise(witnessEvent);
    }
        
    public void AddEvent(WitnessRequest request)
    {
        var witnessEvent = new WitnessEvent
        {
            TagId = request.Number.ToString(),
            Time = request.Time.LocalDateTime,
            Type = request.Type
        };
        Witness.Raise(witnessEvent);
    }

    private DateTime GetSnapshotTime(long epoch)
    {
        var offset = DateTimeOffset.FromUnixTimeMilliseconds(epoch);
        return offset.LocalDateTime;
    }
}

public interface IWitnessEventService
{
    void AddEvent(WitnessEventType type, TagRequest request);
    void AddEvent(WitnessRequest request);
}
