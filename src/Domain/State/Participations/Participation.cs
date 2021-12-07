using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Performances;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.State.Participations
{
    public class Participation : DomainObjectBase<ParticipationException>
    {
        private List<Competition> competitions = new();
        private List<Performance> phasePerformances = new();

        internal void Add(Competition competition) => this.Validate(() =>
        {
            if (this.Competitions.Any())
            {
                var first = this.competitions.First();
                // TODO if first.Configuration != competition.Configuration -> throw
            }
            this.competitions.Add(competition);
        });

        public void Add(Performance performance)
        {
            this.phasePerformances.AddUnique(performance);
        }

        public IReadOnlyList<Competition> Competitions
        {
            get => this.competitions.AsReadOnly();
            private set => this.competitions = value.ToList();
        }
        public IReadOnlyList<Performance> Performances
        {
            get => this.phasePerformances.AsReadOnly();
            private set => this.phasePerformances = value.ToList();
        }
    }
}
