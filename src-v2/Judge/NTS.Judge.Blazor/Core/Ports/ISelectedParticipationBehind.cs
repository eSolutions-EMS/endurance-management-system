using Not.Blazor.Ports;
using NTS.Domain.Core.Entities;

namespace NTS.Judge.Blazor.Core.Ports;

public interface ISelectedParticipationBehind : IObservableBehind
{
    Participation? SelectedParticipation { get; set; }
}
