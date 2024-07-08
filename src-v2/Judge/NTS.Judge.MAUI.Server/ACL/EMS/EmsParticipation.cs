namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsParticipation : EmsDomainBase<EmsParticipationException>
{
    internal EmsParticipation() { }
    internal EmsParticipation(EmsParticipant participant, EmsCompetition competition) : base(default)
    {
        Participant = participant;
        CompetitionConstraint = competition;
        competitionsIds.Add(competition.Id);
    }

    public static EventHandler<EmsParticipation> UpdateEvent;
    internal void RaiseUpdate()
    {
        UpdateEvent?.Invoke(null, this);
    }

    private List<int> competitionsIds = new();
    public EmsParticipant Participant { get; private set; }
    public EmsCompetition CompetitionConstraint { get; internal set; }
    public EmsWitnessEventType UpdateType { get; internal set; }

    public bool IsNotComplete
        => !Participant.LapRecords.Any(x => x.Result?.IsNotQualified ?? false) &&
            (Participant.LapRecords.Count != CompetitionConstraint?.Laps.Count
            || Participant.LapRecords.Last().Result == null);

    public double? Distance
        => CompetitionConstraint
            ?.Laps
            .Select(x => x.LengthInKm)
            .Sum();

    internal void Add(EmsCompetition competition)
    {
        if (CompetitionConstraint == null)
        {
            CompetitionConstraint = competition;
        }
        competitionsIds.Add(competition.Id);
    }
    internal void Remove(int competitionId)
    {
        competitionsIds.Remove(competitionId);
        if (!CompetitionsIds.Any())
        {
            CompetitionConstraint = null;
        }
    }

    public IReadOnlyList<int> CompetitionsIds
    {
        get => competitionsIds.AsReadOnly();
        private set => competitionsIds = value.ToList();
    }

    public void __REMOVE_PERFORMANCES__()
    {
        Participant.__REMOVE_RECORDS__();
    }
}
