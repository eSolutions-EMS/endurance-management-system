using EnduranceJudge.Domain.Aggregates.Manager.PhasePerformances;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.State.PhasePerformances;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Manager.Participations
{
    public class ParticipationManager : ManagerObjectBase
    {
        private readonly Competition competition;
        private readonly Participation participation;
        private readonly List<PhasePerformanceManager> performanceManagers = new();

        internal ParticipationManager(Participant participant)
        {
            this.ParticipantId = participant.Id;
            this.participation = participant.Participation;
            this.competition = this.participation.Competitions.First();

            foreach (var performance in this.participation.Performances)
            {
                var manager = new PhasePerformanceManager(performance);
                this.performanceManagers.Add(manager);
            }
        }

        public PhasePerformanceManager CurrentPerformanceManager
            => this.performanceManagers.Last();

        public int ParticipantId { get; }

        internal void Start()
        {
            var firstPhase = this.competition.Phases.First();
            var phaseEntry = new PhasePerformance(firstPhase, this.competition.StartTime);
            this.participation.Add(phaseEntry);
        }
        internal void UpdatePerformance(DateTime time)
        {
            this.CurrentPerformanceManager.Update(time);
        }
        internal void CompletePerformance()
        {
            this.CurrentPerformanceManager.Complete();
            if (!this.IsComplete(this.participation))
            {
                var restTime = this.CurrentPerformanceManager.Phase.RestTimeInMins;
                var startTime = this.CurrentPerformanceManager.VetGatePassedTime.AddMinutes(restTime);
                this.AddNextPhase(startTime);
            }
        }
        internal void CompletePerformance(string code)
        {
            this.CurrentPerformanceManager.CompleteUnsuccessful(code);
        }

        private void AddNextPhase(DateTime startTime)
        {
            if (!this.CurrentPerformanceManager.IsComplete)
            {
                this.Throw<PhasePerformanceException>("cannot start - previous participation is not complete.");
            }

            var phase = this.competition
                .Phases
                .Skip(this.performanceManagers.Count)
                .FirstOrDefault();
            if (phase == null)
            {
                this.Throw<PhasePerformanceException>("cannot start - no next phase.");
            }

            var performance = new PhasePerformance(phase, startTime);
            var manager = new PhasePerformanceManager(performance);
            this.performanceManagers.Add(manager);
        }

        private bool IsComplete(Participation participation)
        {
            return participation.Performances.Count == this.competition.Phases.Count
                && participation.Performances.All(x => x.Result != null);
        }
    }
}
