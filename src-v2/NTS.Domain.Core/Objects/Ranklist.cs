using NTS.Domain.Configuration;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects.Regional;
using System.Collections.ObjectModel;

namespace NTS.Domain.Core.Objects;

public class Ranklist : ReadOnlyCollection<RankingEntry>
{
    private static FeiRanker _feiRanker = new();
    private static Ranker[] _regionalRankers = [ new BulgariaRanker() ];

    public Ranklist(Ranking ranking, IEnumerable<Participation> participations)
        : base(Rank(ranking.Category, ranking.Entries, participations))
    {
        Name = ranking.Name;
        Category = ranking.Category;
    }

    public string Title => $"{Category}: {Name}";
    public string Name { get; }
    public AthleteCategory Category { get; }

    private static IList<RankingEntry> Rank(AthleteCategory category, IEnumerable<RankingEntry> entries, IEnumerable<Participation> participations)
    {
        var ranker = GetRanker(StaticOptions.Configuration);
        return ranker.Rank(category, entries, participations);
    }

    private static Ranker GetRanker(IRegionalConfiguration? configuration)
    {
        return _regionalRankers.FirstOrDefault(x => x.Country == configuration?.Country) ?? _feiRanker;
    }
}   
