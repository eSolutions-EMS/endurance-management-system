using Core.Models;

namespace Core.Domain.State.EnduranceEvents;

public interface IEnduranceEventState : IIdentifiable
{
    public string Name { get; }
    public string PopulatedPlace { get; }
    public bool HasStarted { get; }
}
