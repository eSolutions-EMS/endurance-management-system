using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Gateways.Persistence.Contracts;

namespace EnduranceJudge.Gateways.Persistence
{
    public class DataContext : IDataContext
    {
        public DataContext()
        {
            this.State = new State();
        }

        public State State { get; }
    }

    public interface IDataContext : ISingletonService
    {
        State State { get; }
    }
}
