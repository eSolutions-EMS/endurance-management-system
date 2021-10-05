using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Gateways.Persistence.Contracts;

namespace EnduranceJudge.Gateways.Persistence
{
    public class DataContext : IDataContext
    {
        public DataContext()
        {
            this.State = new State();
            this.State.Horses.Add(new Horse(default, "a", "b", "c", "d"));
        }

        public State State { get; }
    }

    public interface IDataContext : ISingletonService
    {
        State State { get; }
    }
}
