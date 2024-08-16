using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;
using System.Collections.ObjectModel;

namespace NTS.Domain.Core.Objects;

public class Ranklist : ReadOnlyCollection<RankingEntry>
{
    public Ranklist(Ranking ranking, IEnumerable<Participation> participations)
        : base(Rank(ranking.Entries, participations))
    {
        Name = ranking.Name;
        Category = ranking.Category;
    }

    public string Title => $"{Category}: {Name}";
    public string Name { get; }
    public AthleteCategory Category { get; }

    private static IList<RankingEntry> Rank(IEnumerable<RankingEntry> entries, IEnumerable<Participation> participations)
    {
        var ids = entries.Select(x => x.ParticipationId).ToList();
        var ranked = entries
            .Select(x => new { Entry = x, Participation = participations.First(y => x.ParticipationId == y.Id) })
            .OrderBy(x => x.Participation.IsNotQualified)
            .ThenByDescending(x => !x.Entry.IsRanked)
            .ThenBy(x => x.Participation.Phases.Last().ArriveTime)
            .Select(x => x.Entry)
            .ToList();
        return ranked;
    }
}
