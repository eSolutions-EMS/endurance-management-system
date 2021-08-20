using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.Aggregates.Import.Participants
{
    public class Participant : DomainBase<ImportParticipantException>
    {
        private Participant()
        {
        }

        public Participant(Athlete athlete, Horse horse): base(default)
            => this.Validate(() =>
            {
                this.Athlete = athlete.IsRequired(nameof(athlete));
                this.Horse = horse.IsRequired(nameof(horse));
            });

        public Athlete Athlete { get; private set; }
        public Horse Horse { get; private set; }
    }
}
