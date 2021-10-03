using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Aggregates.State.Phases;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using EnduranceJudge.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.Aggregates.State.Competitions
{
    public class Competition : DomainObjectBase<CompetitionException>, ICompetitionState
    {
        private Competition() {}
        public Competition(int id, CompetitionType type, string name, DateTime startTime) : base(id)
            => this.Validate(() =>
        {
            this.Type = type.IsRequired(TYPE);
            this.Name = name.IsRequired(NAME);
            this.StartTime = startTime
                .IsRequired(START_TIME)
                .IsFutureDate();
        });

        private List<Phase> phases = new();
        public CompetitionType Type { get; private set; }
        public string Name { get; private set; }
        public DateTime StartTime { get; private set; }

        public void Add(Phase phase) => this.Validate(() =>
        {
            this.phases.AddOrUpdateObject(phase);
        });

        public IReadOnlyList<Phase> Phases
        {
            get => this.phases.AsReadOnly();
            private set => this.phases = value.ToList();
        }
    }
}
