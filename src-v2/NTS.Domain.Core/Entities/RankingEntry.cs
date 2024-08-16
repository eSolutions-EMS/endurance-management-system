using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Domain.Core.Entities;

public class RankingEntry : DomainEntity
{
    public RankingEntry(Participation participation, bool isRanked)
    {
        Participation = participation;
        IsRanked = isRanked;
    }

    public Participation Participation { get; private set; }
    public bool IsRanked { get; private set; }
}
