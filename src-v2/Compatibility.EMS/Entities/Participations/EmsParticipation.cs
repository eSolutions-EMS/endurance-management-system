using NTS.Compatibility.EMS.Abstractions;
using NTS.Compatibility.EMS.Entities.Competitions;
using NTS.Compatibility.EMS.Entities.LapRecords;
using NTS.Compatibility.EMS.Entities.Participants;
namespace NTS.Compatibility.EMS.Entities.Participations;

public class EmsParticipation : EmsDomainBase<EmsParticipationException>
{
    [Newtonsoft.Json.JsonConstructor]
    internal EmsParticipation() {}
    public EmsParticipation(EmsParticipant participant, EmsCompetition competition) : base(GENERATE_ID)
    {
        this.Participant = participant;
        this.CompetitionConstraint = competition;
        this.competitionsIds.Add(competition.Id);
    }

    public static EventHandler<EmsParticipation> UpdateEvent;
    internal void RaiseUpdate()
    {
        UpdateEvent?.Invoke(null, this);
    }

    private List<int> competitionsIds = new();
    public EmsParticipant Participant { get; private set; }
    public EmsCompetition CompetitionConstraint { get; internal set; }
    public WitnessEventType UpdateType { get; internal set; }

    public bool IsNotComplete
        => !this.Participant.LapRecords.Any(x => x.Result?.IsNotQualified ?? false) &&
            (this.Participant.LapRecords.Count != this.CompetitionConstraint?.Laps.Count
            || this.Participant.LapRecords.Last().Result == null);

    public double? Distance
        => this.CompetitionConstraint
            ?.Laps
            .Select(x => x.LengthInKm)
            .Sum();

    internal void Add(EmsCompetition competition)
    {
        if (this.CompetitionConstraint == null)
        {
            this.CompetitionConstraint = competition;
        }
        this.competitionsIds.Add(competition.Id);
    }
    internal void Remove(int competitionId)
    {
        this.competitionsIds.Remove(competitionId);
        if (!this.CompetitionsIds.Any())
        {
            this.CompetitionConstraint = null;
        }
    }

    public IReadOnlyList<int> CompetitionsIds
    {
        get => this.competitionsIds.AsReadOnly();
        private set => this.competitionsIds = value.ToList();
    }

    public void __REMOVE_PERFORMANCES__()
    {
        this.Participant.__REMOVE_RECORDS__();
    }
}
