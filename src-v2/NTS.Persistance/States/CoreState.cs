using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;

namespace NTS.Persistence.States;

public class CoreState : NotState,
    ITreeState<Event>,
    ISetState<Official>,
    ISetState<Participation>,
    ISetState<Ranking>,
    ISetState<SnapshotResult>,
    ISetState<Handout>
{
    public Event? Event { get; set; }
    public List<Official> Officials { get; } = [];
    public List<Participation> Participations { get; } = [];
    public List<Ranking> Rankings { get; } = [];
    public List<SnapshotResult> SnapshotResults { get; } = [];
    public List<Handout> Handouts { get; } = [];

    Event? ITreeState<Event>.Root { get => Event; set => Event = value; }
    List<Official> ISetState<Official>.EntitySet => Officials;
    List<Participation> ISetState<Participation>.EntitySet => Participations;
    List<Ranking> ISetState<Ranking>.EntitySet => Rankings;
    List<SnapshotResult> ISetState<SnapshotResult>.EntitySet => SnapshotResults;
    List<Handout> ISetState<Handout>.EntitySet => Handouts;
}
