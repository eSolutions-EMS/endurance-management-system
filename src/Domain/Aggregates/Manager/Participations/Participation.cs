using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Aggregates.Manager.DTOs;
using EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInCompetitions;
using EnduranceJudge.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Manager.Participations
{
    public class Participation : DomainBase<ManagerParticipationException>
    {
        private const string AlreadyStartedMessage = "has already started";

        private readonly IReadOnlyList<CompetitionDto> competitions;
        private readonly List<ParticipationInCompetition> participationsInCompetitions = new();
        private readonly int? maxAverageSpeedInKpH;

        internal Participation(IReadOnlyList<CompetitionDto> competitions, int? maxAverageSpeedInKpH)
        {
            this.maxAverageSpeedInKpH = maxAverageSpeedInKpH;
            this.competitions = competitions;

            this.Validate(this.Start);
        }

        public bool HasExceededSpeedRestriction
            => this.participationsInCompetitions.All(participation => participation.HasExceededSpeedRestriction);

        public bool IsComplete
            => this.participationsInCompetitions.All(participation => participation.IsComplete);

        public IReadOnlyList<ParticipationInCompetition> ParticipationsInCompetitions
            => this.participationsInCompetitions.AsReadOnly();

        private void Start()
            => this.Validate(() =>
            {
                this.participationsInCompetitions.IsEmpty(AlreadyStartedMessage);

                foreach (var participationInCompetition in this.competitions
                    .Select(competition => new ParticipationInCompetition(competition, this.maxAverageSpeedInKpH)))
                {
                    this.participationsInCompetitions.Add(participationInCompetition);
                }
            });
        public void Arrive(DateTime time)
        {
            this.Update(participation => participation.CurrentPhase.Arrive(time));
        }
        public void Inspect(DateTime time)
        {
            this.Update(participation => participation.CurrentPhase.Inspect(time));
        }
        public void ReInspect(DateTime time)
        {
            this.Update(participation => participation.CurrentPhase.ReInspect(time));
        }
        public void CompleteSuccessful()
        {
            this.Update(participation => participation.CompleteSuccessful());
        }
        public void CompleteUnsuccessful(string code)
        {
            this.Update(participation => participation.CompleteUnsuccessful(code));
        }

        private void Update(Action<ParticipationInCompetition> action)
        {
            foreach (var participation in this.participationsInCompetitions
                .Where(competition => !competition.IsComplete))
            {
                action(participation);
            }
        }
    }
}
