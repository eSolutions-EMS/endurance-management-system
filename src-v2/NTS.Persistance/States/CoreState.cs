using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;

namespace NTS.Persistence.States;

public class CoreState : NotState,
    ITreeState<Event>,
    ISetState<Official>,
    ISetState<Participation>,
    ISetState<Ranking>,
    ISetState<SnapshotResult>,
    ISetState<HandoutDocument>
{
    public Event? Event { get; set; }
    public List<Official> Officials { get; } = new();
    public List<Participation> Participations { get; } = new();
    public List<Ranking> Rankings { get; } = new();
    public List<SnapshotResult> SnapshotResults { get; } = new();
    public List<HandoutDocument> Handouts { get; } = new();

    Event? ITreeState<Event>.Root { get => Event; set => Event = value; }
    List<Official> ISetState<Official>.EntitySet => Officials;
    List<Participation> ISetState<Participation>.EntitySet => Participations;
    List<Ranking> ISetState<Ranking>.EntitySet => Rankings;
    List<SnapshotResult> ISetState<SnapshotResult>.EntitySet => SnapshotResults;
    List<HandoutDocument> ISetState<HandoutDocument>.EntitySet => Handouts;
}
