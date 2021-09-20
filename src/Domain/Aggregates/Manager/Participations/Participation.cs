using EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInCompetitions;
using EnduranceJudge.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Manager.Participations
{
    public class Participation : DomainBase<ManagerParticipationException>, IAggregateRoot
    {
        private const string ALREADY_STARTED_MESSAGE = "has already started";

        private List<ParticipationInCompetition> participationsInCompetitions = new();

        private Participation()
        {
        }

        public string Number { get; private set; }
        public IReadOnlyList<ParticipationInCompetition> ParticipationsInCompetitions
        {
            get => this.participationsInCompetitions.AsReadOnly();
            private set => this.participationsInCompetitions = value.ToList();
        }

        public bool HasExceededSpeedRestriction
            => this.participationsInCompetitions.All(participation => participation.HasExceededSpeedRestriction);
        public bool IsComplete => this.participationsInCompetitions.All(participation => !participation.IsNotComplete);

        public void Start()
            => this.Validate(() =>
            {
                // TODO: Fix this check
                // this.participationsInCompetitions.IsEmpty(ALREADY_STARTED_MESSAGE);

                foreach (var participation in this.ParticipationsInCompetitions)
                {
                    participation.StartPhase();
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
            //TODO: If HasExceededSpeedRestrictions ...
            this.Update(participation => participation.CompleteSuccessful());
        }
        public void CompleteUnsuccessful(string code)
        {
            this.Update(participation => participation.CompleteUnsuccessful(code));
        }

        private void Update(Action<ParticipationInCompetition> action)
        {
            foreach (var participation in this.participationsInCompetitions
                .Where(competition => competition.IsNotComplete))
            {
                action(participation);
            }
        }
    }
}
