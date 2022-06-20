using Endurance.Judge.Gateways.API.Requests;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using System;
using System.Collections.Generic;

namespace Endurance.Judge.Gateways.API.Services
{
    public class StateEventService : IStateEventService
    {
        private static readonly Queue<WitnessEvent> Events = new();

        public void AddEvent(WitnessEventType type, TagRequest request)
        {
            var time = this.GetSnapshotTime(request.Epoch);
            var witnessEvent = new WitnessEvent
            {
                Type = type,
                TagId = request.Id.Replace("0", string.Empty),
                Time = time,
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
        Dictionary<int, WitnessEvent> GetEvents();
    }
}
