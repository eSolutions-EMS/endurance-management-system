using EnduranceJudge.Domain.Aggregates.Event.Participants;
using EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInCompetitions;
using EnduranceJudge.Domain.Aggregates.Manager.ParticipationsInPhases;
using EnduranceJudge.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Strings.Domain;

namespace EnduranceJudge.Domain.Aggregates.Manager.Participations
{
    public class Participation : DomainBase<ManagerParticipationException>, IAggregateRoot
    {
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

        public DateTime StartTime => this.ParticipationsInCompetitions
            .First()
            .StartTime;
        public bool HasNotStarted
            => !this.IsComplete
                && this.ParticipationsInCompetitions.All(x => !x.ParticipationsInPhases.Any());
        public bool CanArrive => !this.IsComplete && this.AnyParticipationInPhase(pip => pip.CanArrive);
        public bool CanInspect => !this.IsComplete && this.AnyParticipationInPhase(pip => pip.CanInspect);
        public bool CanReInspect => !this.IsComplete && this.AnyParticipationInPhase(pip => pip.CanReInspect);
        public bool CanComplete => !this.IsComplete && this.AnyParticipationInPhase(pip => pip.CanComplete);
        public bool HasExceededSpeedRestriction
            => this.participationsInCompetitions.All(participation => participation.HasExceededSpeedRestriction);
        public bool IsComplete => this.participationsInCompetitions.All(participation => !participation.IsNotComplete);

        public void Start()
            => this.Validate(() =>
            {
                if (!this.HasNotStarted)
                {
                    return;
                    // TODO: AddValidation to these checks;
                }

                foreach (var participation in this.ParticipationsInCompetitions)
                {
                    participation.StartPhase();
                }
            });
        public void UpdateProgress(DateTime time)
            => this.Validate(() =>
            {
                if (this.HasNotStarted)
                {
                    throw new ParticipantException
                    {
                        DomainMessage = PARTICIPATION_HAS_NOT_STARTED,
                    };
                }
                if (this.CanArrive)
                {
                    this.Arrive(time);
                }
                else if (this.CanInspect)
                {
                    this.Inspect(time);
                }
                else if (this.CanReInspect)
                {
                    this.ReInspect(time);
                }
            });
        internal void Arrive(DateTime time)
        {
            this.Update(participation => participation.CurrentPhase.Arrive(time));
        }
        internal void Inspect(DateTime time)
        {
            this.Update(participation => participation.CurrentPhase.Inspect(time));
        }
        internal void ReInspect(DateTime time)
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

        private bool AnyParticipationInPhase(Func<ParticipationInPhase, bool> predicate)
        {
            return this.ParticipationsInCompetitions.Any(pic =>
                pic.ParticipationsInPhases.Any(predicate));
        }
    }
}
