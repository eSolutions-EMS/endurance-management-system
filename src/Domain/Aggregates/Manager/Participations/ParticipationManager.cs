using EnduranceJudge.Domain.Aggregates.Manager.PhasePerformances;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Domain.State.Phases;
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
                var manager = new PerformanceManager(performance);
                this.performanceManagers.Add(manager);
                if (performance.Result == null)
                {
                    this.CurrentPerformance = manager;
                }
            }
        }

        public PerformanceManager CurrentPerformance { get; private set; }

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
            this.AddPerformanceManager(firstPhase, this.competition.StartTime);
        }
        internal void UpdatePerformance(DateTime time)
        {
            if (this.CurrentPerformance == null)
            {
                this.StartNextPerformance();
            }
            this.CurrentPerformance.Update(time);
        }
        internal void CompletePerformance()
        {
            DateTime? nextPhaseStartTime = null;
            if (!this.CurrentPerformance.Phase.IsFinal)
            {
                var restTime = this.CurrentPerformance.Phase.RestTimeInMins;
                nextPhaseStartTime = this.CurrentPerformance.VetGatePassedTime.AddMinutes(restTime);
            }
            this.CurrentPerformance.Complete(nextPhaseStartTime);
            this.CurrentPerformance = null;
        }
        internal void CompletePerformance(string code)
        {
            this.CurrentPerformance.CompleteUnsuccessful(code);
            this.CurrentPerformance = null;
        }
        internal void RequireInspection()
        {
            this.CurrentPerformance.RequireInspection();
        }
        internal void CompleteRequiredInspection()
        {
            this.CurrentPerformance.CompleteRequiredInspection();
        }

        private void StartNextPerformance()
        {
            if (this.IsComplete)
            {
                throw new InvalidOperationException(CANNOT_START_NEXT_PERFORMANCE_PARTICIPATION_IS_COMPLETE);
            }

            var phase = this.competition
                .Phases
                .Skip(this.performanceManagers.Count)
                .FirstOrDefault();
            if (phase == null)
            {
                throw new InvalidOperationException(CANNOT_START_PERFORMANCE_NO_PHASE);
            }
            var previousPerformance = this.performanceManagers.Last();
            var startTime = previousPerformance.NextPerformanceStartTime;
            if (startTime == null)
            {
                throw new InvalidOperationException(CANNOT_START_PERFORMANCE_NO_START_TIME);
            }
            this.AddPerformanceManager(phase, startTime.Value);
        }

        private bool IsComplete
            => this.participation.Performances.Count == this.competition.Phases.Count
                && this.participation.Performances.All(x => x.Result != null);

        private void AddPerformanceManager(Phase phase, DateTime startTime)
        {
            var performance = new Performance(phase, startTime);
            this.participation.Add(performance);
            var performanceManager = new PerformanceManager(performance);
            this.performanceManagers.Add(performanceManager);
            this.CurrentPerformance = performanceManager;
        }
    }
}
