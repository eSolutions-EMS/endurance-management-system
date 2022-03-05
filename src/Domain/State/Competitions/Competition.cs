using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Phases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.State.Competitions
{
    public class Competition : DomainObjectBase<CompetitionException>, ICompetitionState
    {
        private Competition() {}
        public Competition(CompetitionType type, string name) : base(GENERATE_ID)
        {
            this.Type = type;
            this.Name = name;
            this.StartTime = DateTime.Now;
        }
        public Competition(ICompetitionState state) : base(GENERATE_ID)
        {
            this.Type = state.Type;
            this.Name = state.Name;
            this.StartTime = state.StartTime;
        }

        private List<Phase> phases = new();
        public CompetitionType Type { get; internal set; }
        public string Name { get; internal set; }
        public DateTime StartTime { get; set; }

        public void Save(Phase phase)
        {
            this.phases.AddOrUpdate(phase);
        }

        public IReadOnlyList<Phase> Phases
        {
            get => this.phases.OrderBy(x => x.OrderBy).ToList().AsReadOnly();
            private set => this.phases = value.ToList();
        }
    }
}
