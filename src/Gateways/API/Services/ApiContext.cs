using EnduranceJudge.Application;
using EnduranceJudge.Domain.State;

namespace Endurance.Judge.Gateways.API.Services
{
    public class ApiContext : IApiContext, IStateContext
    {
        public State ApiState { get; set; } = new State();
        public IState State => this.ApiState;
    }

    public interface IApiContext
    {
        State ApiState { get; }
    }
}
