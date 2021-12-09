using EnduranceJudge.Domain.Aggregates.Manager.PhasePerformances;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.State.Performances;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.Strings.Domain.Manager.Participation;

namespace EnduranceJudge.Domain.Aggregates.Manager.Participations
{
    public class ParticipationManager : ManagerObjectBase
    {
        private readonly Competition competition;
        private readonly Participation participation;
        private readonly List<PerformanceManager> performanceManagers = new();

        internal ParticipationManager(Participant participant)
        {
            this.Number = participant.Number;
            this.participation = participant.Participation;
            this.competition = this.participation.Competitions.First();

            foreach (var performance in this.participation.Performances)
            {
                this.AddPerformanceManager(performance);
            }
        }

        public PerformanceManager CurrentPerformanceManager
            => this.performanceManagers.LastOrDefault();

        public int Number { get; }

        internal void Start()
        {
            if (this.participation.Performances.Any())
            {
                this.Throw<ParticipationException>(HAS_ALREADY_STARTED);
            }
            var firstPhase = this.competition.Phases.FirstOrDefault();
            if (firstPhase == null)
            {
                this.Throw<ParticipantException>(CANNOT_START_COMPETITION_WITHOUT_PHASES);
            }
            var performance = new Performance(firstPhase, this.competition.StartTime);
            this.participation.Add(performance);
            this.AddPerformanceManager(performance);
        }
        internal void UpdatePerformance(DateTime time)
        {
            if (this.CurrentPerformanceManager == null)
            {
                this.Throw<ParticipationException>(HAS_NOT_STARTED);
            }
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
        internal void RequireInspection()
        {
            this.CurrentPerformanceManager.RequireInspection();
        }
        internal void CompleteRequiredInspection()
        {
            this.CurrentPerformanceManager.CompleteRequiredInspection();
        }

        private void AddNextPhase(DateTime startTime)
        {
            if (!this.CurrentPerformanceManager.IsComplete)
            {
                this.Throw<PerformanceException>("cannot start - previous participation is not complete.");
            }

            var phase = this.competition
                .Phases
                .Skip(this.performanceManagers.Count)
                .FirstOrDefault();
            if (phase == null)
            {
                this.Throw<PerformanceException>("cannot start - no next phase.");
            }

            var performance = new Performance(phase, startTime);
            this.participation.Add(performance);
            var manager = new PerformanceManager(performance);
            this.performanceManagers.Add(manager);
        }

        private bool IsComplete(Participation participation)
        {
            return participation.Performances.Count == this.competition.Phases.Count
                && participation.Performances.All(x => x.Result != null);
        }

        private void AddPerformanceManager(Performance performance)
        {
            var performanceManager = new PerformanceManager(performance);
            this.performanceManagers.Add(performanceManager);
        }
    }
}
