using Not.Blazor.Ports;
using NTS.Domain.Core.Entities;

namespace NTS.Judge.Blazor.Core.Dashboards;

public interface IParticipationContext : IObservableBehind
{
    Participation? SelectedParticipation { get; set; }
}
