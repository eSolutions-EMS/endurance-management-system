using EnduranceJudge.Domain.AggregateRoots.Ranking.Aggregates;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Ranking;

public class RankingRoot : IAggregateRoot
{
    private readonly List<CompetitionResultAggregate> competitions = new();

    public RankingRoot(IState state)
    {
        if (state.Event == default)
        {
            return;
        }
        var competitionsIds = state.Participations
            .SelectMany(x => x.CompetitionsIds)
            .Distinct()
            .ToList();
        foreach (var id in competitionsIds)
        {
            var competition = state.Event.Competitions.FindDomain(id);
            var participations = state.Participations
                .Where(x => x.CompetitionsIds.Contains(competition.Id))
                .ToList();
            var listing = new CompetitionResultAggregate(state.Event, competition, participations);
            this.competitions.Add(listing);
        }
    }

    public CompetitionResultAggregate GetCompetition(int competitionId)
    {
        var aggregate = this.competitions.First(x => x.Id == competitionId);
        return aggregate;
    }

    public IReadOnlyList<CompetitionResultAggregate> Competitions => this.competitions.AsReadOnly();
}
