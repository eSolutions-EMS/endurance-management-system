using EnduranceJudge.Core.Models;

namespace EnduranceJudge.Domain.State.EnduranceEvents
{
    public interface IEnduranceEventState : IIdentifiable
    {
        public string Name { get; }
        public string PopulatedPlace { get; }
    }
}
