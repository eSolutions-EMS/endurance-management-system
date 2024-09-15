using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects.Regional;

internal class FeiRanker : Ranker
{
    public override IList<RankingEntry> Rank(Ranking ranking, IEnumerable<Participation> participations)
    {

        var ids = ranking.Entries.Select(x => x.ParticipationId).ToList();
        return BaseOrder(ranking.Entries, participations)
            .ThenBy(x => x.Participation.Phases.Last().ArriveTime)
            .Select(x => x.RankingEntry)
            .ToList();
    }

    protected IOrderedEnumerable<Pair> BaseOrder(IEnumerable<RankingEntry> entries, IEnumerable<Participation> participations)
    {
        var ids = entries.Select(x => x.ParticipationId).ToList();
        return entries
            .Select(x => new Pair(x, participations.First(y => x.ParticipationId == y.Id)))
            .OrderBy(x => x.Participation.IsNotQualified())
            .ThenByDescending(x => !x.RankingEntry.IsRanked);
    }
}

internal abstract class Ranker
{
    public Country? Country { get; protected set; }
    public abstract IList<RankingEntry> Rank(Ranking ranking, IEnumerable<Participation> participations);

    public record Pair(RankingEntry RankingEntry, Participation Participation);
}
