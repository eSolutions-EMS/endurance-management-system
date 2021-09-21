using EnduranceJudge.Domain.Aggregates.Ranking.Participations;
using EnduranceJudge.Domain.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Ranking.Competitions
{
    public class Competition : DomainBase<RankingCompetitionException>
    {
        private List<Participation> participations = new();

        private Competition()
        {
        }

        public int LengthInKilometers { get; private set; }

        public IReadOnlyList<Participation> Participations
        {
            get => this.participations.AsReadOnly();
            private set => this.participations = value.ToList();
        }
    }
}
