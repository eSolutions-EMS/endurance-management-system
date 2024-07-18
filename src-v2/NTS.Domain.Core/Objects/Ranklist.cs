using NTS.Domain.Core.Entities;
using System.Collections.ObjectModel;

namespace NTS.Domain.Core.Objects;

public class Ranklist : ReadOnlyCollection<ClassificationEntry>
{
    public Ranklist(Classification classification)
        : base(classification.Category == AthleteCategory.Senior
            ? RankSeniors(classification.Entries)
            : RankOthers(classification.Entries))
    {
        Classification = classification;
    }

    public Classification Classification { get; }

    private static IList<ClassificationEntry> RankSeniors(IEnumerable<ClassificationEntry> entry)
    {
        var ranked = OrderByNotQualifiedThenNotRanked(entry)
            .ThenBy(x => x.Participation.Phases.Last().ArriveTime)
            .ToList();
        return ranked;
    }

    private static IList<ClassificationEntry> RankOthers(IEnumerable<ClassificationEntry> entry)
    {
        var ranked = OrderByNotQualifiedThenNotRanked(entry)
            .ThenBy(x => x.Participation.Total?.RecoverySpan)
            .ToList();
        return ranked;
    }

    private static IOrderedEnumerable<ClassificationEntry> OrderByNotQualifiedThenNotRanked(IEnumerable<ClassificationEntry> entry)
    {
        return entry
            .OrderBy(x => x.Participation.IsNotQualified)
            .ThenByDescending(x => !x.IsRanked);
    }
}
