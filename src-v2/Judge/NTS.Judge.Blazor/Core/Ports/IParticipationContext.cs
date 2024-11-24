using Not.Blazor.Ports;
using NTS.Domain.Core.Entities;

namespace NTS.Judge.Blazor.Core.Ports;

public interface IParticipationContext : IObservableBehind
{
    Participation? SelectedParticipation { get; set; }
}
