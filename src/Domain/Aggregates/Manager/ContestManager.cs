using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.Aggregates.Manager.Participants;
using EnduranceJudge.Domain.Aggregates.Manager.Performances;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Localization.Translations;
using System;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Manager
{
    public class ContestManager : ManagerObjectBase, IAggregateRoot
    {
        private readonly IState state;

        public ContestManager()
        {
            this.state = StaticProvider.GetService<IState>();
            // Check is necessary due to Prism's initialization logic which uses reflection
            // to generate instances of views as part of the startup process.
            // Does views are not used in the actual views during the application use cycle
            if (this.state?.Event == null)
            {
                return;
            }
        }

        public void Start()
        {
            var participants = this.state
                .Participants
                .Select(x => new ParticipantManager(x))
                .ToList();
            foreach (var participant in participants)
            {
                participant.Start();
            }
        }

        public void UpdatePerformance(int number, DateTime time)
        {
            var participant = this.GetParticipant(number);
            participant.UpdatePerformance(time);
        }
        public void CompletePerformance(int number)
        {
            var participant = this.GetParticipant(number);
            participant.CompletePerformance();
        }
        public void CompletePerformance(int number, string code)
        {
            var participant = this.GetParticipant(number);
            var performance = participant.GetActivePerformance();
            performance.Complete(code);
        }
        public void RequireInspection(int number)
        {
            var participant = this.GetParticipant(number);
            var performance = participant.GetActivePerformance();
            performance.RequireInspection();
        }
        public void CompleteRequiredInspection(int number)
        {
            var participant = this.GetParticipant(number);
            var performance = participant.GetActivePerformance();
            performance.CompleteRequiredInspection();
        }
        public void EditPerformance(IPerformanceState state)
        {
            var performance = this.state
                .Participants
                .Select(part => part.Participation)
                .SelectMany(participant => participant.Performances)
                .FirstOrDefault(perf => perf.Equals(state));
            var manager = new PerformanceManager(performance);
            manager.Edit(state);
        }

        private ParticipantManager GetParticipant(int number)
        {
            var participant = this.state
                .Participants
                .FirstOrDefault(x => x.Number == number);
            if (participant == null)
            {
                var message = string.Format(Messages.PARTICIPANT_NUMBER_NOT_FOUND_TEMPLATE, number);
                throw new ParticipantException { DomainMessage = message };
            }
            var manager = new ParticipantManager(participant);
            return manager;
        }
    }
}
