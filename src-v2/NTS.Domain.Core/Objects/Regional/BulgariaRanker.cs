using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects.Regional;

internal class BulgariaRanker : FeiRanker
{
    public BulgariaRanker()
    {
        Country = new Country("BGR", "Bulgaria");
    }

    public override IList<RankingEntry> Rank(AthleteCategory category, IEnumerable<RankingEntry> entries, IEnumerable<Participation> participations)
    {
        if (category != AthleteCategory.Senior)
        {
            return BaseOrder(entries, participations)
                .ThenBy(x => x.Participation.GetTotal()?.RecoveryIntervalWithoutFinal)
                .Select(x => x.RankingEntry)
                .ToList();
        }
        else
        {
            return base.Rank(category, entries, participations);
        }
    }
}