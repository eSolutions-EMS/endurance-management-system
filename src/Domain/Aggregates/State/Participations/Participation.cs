using EnduranceJudge.Domain.Aggregates.State.Competitions;
using EnduranceJudge.Domain.Aggregates.State.PhaseEntries;
using EnduranceJudge.Domain.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.State.Participations
{
    public class Participation : DomainObjectBase<ParticipationException>
    {
        public Participation() {}

        private List<int> competitionIds = new();
        public List<PhaseEntry> PhaseEntries { get; } = new();

        public void Add(Competition competition) => this.Validate(() =>
        {
            if (this.CompetitionIds.Any())
            {
                var first = this.competitionIds.First();
                // TODO if first.Configuration != competition.Configuration -> throw
            }
            this.competitionIds.Add(competition.Id);
        });

        public IReadOnlyList<int> CompetitionIds
        {
            get => this.competitionIds.AsReadOnly();
            private set => this.competitionIds = value.ToList();
        }
    }
}
