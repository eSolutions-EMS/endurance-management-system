using NTS.Domain.Core.Aggregates;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Enums;

namespace NTS.Judge.Core.Start.Factories;

public class RankingFactory
{
    public static Ranking Create(
        Competition competition,
        AthleteCategory athleteCategory,
        IEnumerable<RankingEntry> rankingEntries
    )
    {
        var ranking = new Ranking(competition, athleteCategory, rankingEntries);
        return ranking;
    }
}
