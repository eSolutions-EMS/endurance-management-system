using EnduranceJudge.Domain.Aggregates.Import.Participants;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Import.Competitions
{
    public class Competition : DomainBase<ImportCompetitionException>
    {
        private Competition()
        {
        }

        public Competition(string name, List<Participant> participants): base(default)
            => this.Validate(() =>
            {
                this.Name = name.IsRequired(nameof(name));
                this.participants = participants.IsRequired(nameof(participants));
                this.Type = CompetitionType.International;
            });

        public CompetitionType Type { get; private set; }
        public string Name { get; private set; }
        private List<Participant> participants;
        public IReadOnlyList<Participant> Participants
        {
            get => this.participants.AsReadOnly();
            private set => this.participants = value.ToList();
        }
    }
}
