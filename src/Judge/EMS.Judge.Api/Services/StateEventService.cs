using EMS.Core.ConventionalServices;
using EMS.Core.Domain.AggregateRoots.Manager;
using EMS.Judge.Api.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Judge.Api.Services
{
    public class StateEventService : IStateEventService
    {
        private static readonly Queue<WitnessEvent> Events = new();

        public void AddEvent(WitnessEventType type, TagRequest request)
        {
            var time = this.GetSnapshotTime(request.Epoch);
            var previous = Events.FirstOrDefault(x => x.TagId == request.Id);
            if (previous is not null && time - previous.Time < TimeSpan.FromMinutes(1))
            {
                return;
            }
            var witnessEvent = new WitnessEvent
            {
                Type = type,
                TagId = request.Id,
                Time = time,
            };
            Events.Enqueue(witnessEvent);
        }
        
        public void AddEvent(WitnessRequest request)
        {
            var witnessEvent = new WitnessEvent
            {
                TagId = request.Number.ToString(),
                Time = request.Time.LocalDateTime,
                Type = request.Type
            };
            Events.Enqueue(witnessEvent);
        }

        public Dictionary<int, WitnessEvent> GetEvents()
        {
            var result = new Dictionary<int, WitnessEvent>();
            var amount = Events.Count;
            for (var i = 0; i < amount; i++)
            {
                result.Add(i, Events.Dequeue());
            }
            return result;
        }

        private DateTime GetSnapshotTime(long epoch)
        {
            var offset = DateTimeOffset.FromUnixTimeMilliseconds(epoch);
            return offset.LocalDateTime;
        }
    }

    public interface IStateEventService : ITransientService
    {
        void AddEvent(WitnessEventType type, TagRequest request);
        void AddEvent(WitnessRequest request);
        Dictionary<int, WitnessEvent> GetEvents();
    }
}
