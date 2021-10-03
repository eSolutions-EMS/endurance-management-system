using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Domain.States;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.State.Participants
{
    // TODO: Rename to Contestant
    public class Participant : DomainObjectBase<ParticipantException>, IParticipantState
    {
        public const int DEFAULT_MAX_AVERAGE_SPEED = 16;

        private Participant() {}
        public Participant(int id, int number, Horse horse, Athlete athlete, string rfId, int? maxAverageSpeedInKmPh)
            : base(id)
            => this.Validate(() =>
        {
            this.RfId = rfId;
            this.MaxAverageSpeedInKmPh = maxAverageSpeedInKmPh;
            this.Number = number.IsRequired(NUMBER);
            this.Horse = horse.IsRequired(HORSE);
            this.Athlete = athlete.IsRequired(ATHLETE);
        });

        public string RfId { get; private set; }
        public int Number { get; private set; }
        public int? MaxAverageSpeedInKmPh { get; private set; }
        public Horse Horse { get; private set; }
        public Athlete Athlete { get; private set; }
        public Participation Participation { get; } = new();
    }
}
