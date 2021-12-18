using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Performances;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Translations.Messages;

namespace EnduranceJudge.Domain.State.Participations
{
    public class Participation : DomainObjectBase<ParticipationException>
    {
        private List<Competition> competitions = new();
        private List<Performance> performances = new();

        internal void Add(Competition competition) => this.Validate(() =>
        {
            if (this.Competitions.Any())
            {
                var newCompetitionName = competition.Name;
                var first = this.competitions.First();
                if (first.Phases.Count != competition.Phases.Count)
                {
                    var message = string.Format(CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_COUNT, newCompetitionName);
                    this.Throw(message);
                }
                for (var i = 0; i <= first.Phases.Count; i++)
                {
                    var existingPhase = first.Phases[i];
                    var newPhase = competition.Phases[i];
                    if (!existingPhase.LengthInKm.PreciseEquals(newPhase.LengthInKm))
                    {
                        var message = string.Format(
                            CANNOT_ADD_PARTICIPATION_DIFFERENT_PHASE_LENGTHS,
                            newCompetitionName);
                        this.Throw(message);
                    }
                }
            }
            this.competitions.AddUnique(competition);
        });
        internal void Remove(Competition competition)
        {
            this.competitions.Remove(competition);
        }
        internal void Add(Performance performance)
        {
            this.performances.AddUnique(performance);
        }

        public IReadOnlyList<Competition> Competitions
        {
            get => this.competitions.AsReadOnly();
            private set => this.competitions = value.ToList();
        }
        public IReadOnlyList<Performance> Performances
        {
            get => this.performances.AsReadOnly();
            private set => this.performances = value.ToList();
        }
    }
}
