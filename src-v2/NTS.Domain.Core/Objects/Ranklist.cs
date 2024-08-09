using NTS.Domain.Core.Entities;
using System.Collections.ObjectModel;

namespace NTS.Domain.Core.Objects;

public class Ranklist : ReadOnlyCollection<RankingEntry>
{
    public Ranklist(Ranking ranking)
        : base(ranking.Category == AthleteCategory.Senior
            ? RankSeniors(ranking.Entries)
            : RankOthers(ranking.Entries))
    {
        Name = ranking.Name;
        Category = ranking.Category;
    }

    public string Title => $"{Category}: {Name}";
    public string Name { get; }
    public AthleteCategory Category { get; }

    private static IList<RankingEntry> RankSeniors(IEnumerable<RankingEntry> entry)
    {
        var ranked = OrderByNotQualifiedThenNotRanked(entry)
            .ThenBy(x => x.Participation.Phases.Last().ArriveTime)
            .ToList();
        return ranked;
    }

    private static IList<RankingEntry> RankOthers(IEnumerable<RankingEntry> entry)
    {
        var ranked = OrderByNotQualifiedThenNotRanked(entry)
            .ThenBy(x => x.Participation.Total?.RecoveryInterval)
            .ToList();
        return ranked;
    }

    private static IOrderedEnumerable<RankingEntry> OrderByNotQualifiedThenNotRanked(IEnumerable<RankingEntry> entry)
    {
        return entry
            .OrderBy(x => x.Participation.IsNotQualified)
            .ThenByDescending(x => !x.IsRanked);
    }
}
