using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Aggregates.State.Participants;
using EnduranceJudge.Domain.Aggregates.State.Phases;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.State.Competitions
{
    public class Competition : DomainObjectBase<CompetitionException>, ICompetitionState
    {
        public Competition(ICompetitionState state) : base(state.Id) => this.Validate(() =>
        {
            this.Type = state.Type.IsRequired(nameof(state.Type));
            this.Name = state.Name.IsRequired(nameof(state.Name));
            this.StartTime = state.StartTime
                .IsRequired(nameof(state.StartTime))
                .IsFutureDate();
        });

        private List<Phase> phases = new();
        private List<Participant> participants = new();
        public CompetitionType Type { get; private set; }
        public string Name { get; private set; }
        public DateTime StartTime { get; private set; }

        public void Add(Phase phase) => this.Validate(() =>
        {
            this.phases.AddOrUpdateObject(phase);
        });
        public void Add(Participant participant) => this.Validate(() =>
        {
            this.participants.AddOrUpdateObject(participant);
        });

        public IReadOnlyList<Phase> Phases
        {
            get => this.phases.AsReadOnly();
            private set => this.phases = value.ToList();
        }
        public IReadOnlyList<Participant> Participants
        {
            get => this.participants.AsReadOnly();
            private set => this.participants = value.ToList();
        }
    }
}
