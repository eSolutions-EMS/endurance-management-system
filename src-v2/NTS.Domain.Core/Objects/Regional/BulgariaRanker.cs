using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects.Regional;

internal class BulgariaRanker : FeiRanker
{
    public BulgariaRanker()
    {
        CountryIsoCode = "BGR";
    }

    public override IList<RankingEntry> Rank(Ranking ranking, IEnumerable<Participation> participations)
    {
        if (ranking.Category != AthleteCategory.Senior)
        {
            return OrderByNotEliminatedAndRanked(ranking.Entries, participations)
                .ThenBy(x => x.Participation.GetTotal()?.RecoveryIntervalWithoutFinal)
                .Select(x => x.RankingEntry)
                .ToList();
        }
        else
        {
            return base.Rank(ranking, participations);
        }
    }
}