using Not.Blazor.Ports;
using NTS.Domain.Core.Entities;

namespace NTS.Judge.Blazor.Core.Ports;

public interface ISelectedParticipationBehind : IObservableBehind
{
    // TODO: this should probably be removed and Participations can be returned from Start instead
    IEnumerable<Participation> Participations { get; }
    Participation? SelectedParticipation { get; set; }
    IReadOnlyList<int> RecentlyProcessed { get; }
}
