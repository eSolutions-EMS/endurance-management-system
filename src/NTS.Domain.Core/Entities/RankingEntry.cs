using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Domain.Core.Entities;

public class RankingEntry : DomainEntity
{
    private RankingEntry(int id) : base(id)
    {
    }
    public RankingEntry(int participationId, bool isRanked)
    {
        ParticipationId = participationId;
        IsRanked = isRanked;
    }

    public int ParticipationId { get; private set; }
    public bool IsRanked { get; private set; }

    public override string ToString()
    {
        return $"IsRanked: {IsRanked}, {ParticipationId}";
    }
}
