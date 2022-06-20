using System;

namespace Endurance.Judge.Gateways.API.Models
{
    public class JudgeEvent
    {
        public JudgeEventType Type { get; init; }
        public string TagId { get; init; }
        public DateTime Time { get; init; }
    }

    public enum JudgeEventType
    {
        Invalid = 0,
        EnterVet = 1,
        Finish = 2,
    }
}
