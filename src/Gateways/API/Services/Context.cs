using EnduranceJudge.Application;

namespace Endurance.Judge.Gateways.API.Services
{
    public class Context : IReadonlyContext
    {
        public State State { get; set; }
    }

    public interface IReadonlyContext
    {
        State State { get; }
    }
}
