using Not.Blazor.CRUD.Forms.Ports;
using Not.Blazor.Ports;

namespace NTS.Judge.Blazor.Setup.EnduranceEvents;

public interface IEnduranceEventBehind : IFormBehind<EnduranceEventFormModel>, IObservableBehind
{
    EnduranceEventFormModel? Model { get; }
}
