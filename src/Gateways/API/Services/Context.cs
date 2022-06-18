using EnduranceJudge.Application;
using EnduranceJudge.Core.ConventionalServices;

namespace Endurance.Judge.Gateways.API.Services
{
    public class Context : IContext
    {
        public State State { get; set; }
    }

    public interface IContext : ISingletonService
    {
        State State { get; }
    }
}
