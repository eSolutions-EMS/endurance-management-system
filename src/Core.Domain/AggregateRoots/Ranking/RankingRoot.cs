using EMS.Core.Domain.AggregateRoots.Ranking.Aggregates;
using EMS.Core.Domain.Core.Models;
using EMS.Core.Domain.State;
using EMS.Core.Domain.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Core.Domain.AggregateRoots.Ranking;

public class RankingRoot : IAggregateRoot
{
    private readonly List<CompetitionResultAggregate> competitions = new();

    public RankingRoot(IStateContext stateContext)
    {
        var state = stateContext.State;
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
