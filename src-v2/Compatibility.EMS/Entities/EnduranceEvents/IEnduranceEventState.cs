using NTS.Compabitility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.EnduranceEvents;

public interface IEnduranceEventState : IIdentifiable
{
    public string Name { get; }
    public string PopulatedPlace { get; }
    public bool HasStarted { get; }
}
