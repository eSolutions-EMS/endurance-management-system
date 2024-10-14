using Not.Localization;
using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Domain.Core.Entities;

public class RankingEntry : DomainEntity
{
    private RankingEntry(int id) : base(id)
    {
    }
    public RankingEntry(Participation participation, bool isNotRanked)
    {
        Participation = participation;
        IsNotRanked = isNotRanked;
    }

    public Participation Participation { get; private set; }
    public bool IsNotRanked { get; private set; } 

    public override string ToString()
    {
        var message = IsNotRanked
            ? $"({"not ranked".Localize()}) "
            : "";
        return message + Participation.ToString();
    }
}
