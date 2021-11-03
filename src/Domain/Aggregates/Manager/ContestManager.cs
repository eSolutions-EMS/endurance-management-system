using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Manager
{
    public class ContestManager : ManagerObjectBase, IAggregateRoot
    {
        private readonly List<ParticipationManager> participationManagers = new();

        public ContestManager()
        {
            var state = StaticProvider.GetService<IState>();
            foreach (var participant in state.Participants)
            {
                var manager = new ParticipationManager(participant);
                this.participationManagers.Add(manager);
            }
        }

        public void Start()
        {
            foreach (var manager in this.participationManagers)
            {
                manager.Start();
            }
        }

        public void UpdatePerformance(int participantId, DateTime time)
        {
            var participation = this.GetParticipation(participantId);
            participation.UpdatePerformance(time);
        }
        public void CompletePerformance(int participantId)
        {
            var participation = this.GetParticipation(participantId);
            participation.CompletePerformance();
        }
        public void CompletePerformance(int participantId, string code)
        {
            var participation = this.GetParticipation(participantId);
            participation.CompletePerformance(code);
        }

        private ParticipationManager GetParticipation(int participantId)
        {
            return this.participationManagers.First(x => x.ParticipantId == participantId);
        }
    }
}
