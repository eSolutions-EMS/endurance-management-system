using EnduranceJudge.Application.Models;
using Endurance.Judge.Gateways.API.Requests;
using EnduranceJudge.Core.ConventionalServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endurance.Judge.Gateways.API.Services
{
    public class JudgeEventQueue : IJudgeEventQueue
    {
        private readonly IJudgeEventExecutor eventExecutor;
        private readonly Queue<WitnessEvent> events = new();

        public JudgeEventQueue(IJudgeEventExecutor eventExecutor)
        {
            this.eventExecutor = eventExecutor;
        }
        
        public void AddEvent(WitnessEventType type, TagRequest request)
        {
            var time = this.GetSnapshotTime(request.Epoch);
            var judgeEvent = new WitnessEvent
            {
                Type = type,
                TagId = request.Id.Replace("0", string.Empty),
                Time = time,
            };
            this.events.Enqueue(judgeEvent);
        }
        public void ExecuteEvents()
        {
            while (this.events.Any())
            {
                var judgeEvent = this.events.Dequeue();
                this.eventExecutor.Execute(judgeEvent);
            }
        }

        private DateTime GetSnapshotTime(long epoch)
        {
            var offset = DateTimeOffset.FromUnixTimeMilliseconds(epoch);
            return offset.LocalDateTime;
        }
    }

    public interface IJudgeEventQueue : ISingletonService
    {
        void AddEvent(WitnessEventType type, TagRequest request);
        void ExecuteEvents();
    }
}
