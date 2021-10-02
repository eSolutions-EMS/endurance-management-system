using EnduranceJudge.Domain.Aggregates.State.Competitions;
using EnduranceJudge.Domain.Aggregates.State.PhaseEntries;
using EnduranceJudge.Domain.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.State.Participations
{
    public class Participation : DomainObjectBase<ParticipationException>
    {
        private readonly List<Competition> competitions = new();
        public IReadOnlyList<Competition> Competitions => this.competitions.AsReadOnly();

        public List<PhaseEntry> PhaseEntries { get; } = new();

        public void Add(Competition competition) => this.Validate(() =>
        {
            if (this.Competitions.Any())
            {
                var first = this.competitions.First();
                // TODO if first.Configuration != competition.Configuration -> throw
            }
            this.competitions.Add(competition);
        });
    }
}
