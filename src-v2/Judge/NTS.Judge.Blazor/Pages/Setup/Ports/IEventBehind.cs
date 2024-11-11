using Not.Blazor.CRUD.Forms.Ports;
using Not.Blazor.Observable.Ports;
using NTS.Judge.Blazor.Setup.Events;

namespace NTS.Judge.Blazor.Pages.Setup.Ports;

public interface IEnduranceEventBehind : IFormBehind<EnduranceEventFormModel>, IObservableBehind
{
    EnduranceEventFormModel? Model { get; }
}
