using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.States;

namespace EnduranceJudge.Domain.Aggregates.Event.Participants
{
    public class Participant : DomainBase<ParticipantException>, IParticipantState
    {
        public const int DEFAULT_MAX_AVERAGE_SPEED = 16;

        public Participant(IParticipantState data) : base(data.Id)
        {
            this.Validate(() =>
            {
                this.RfId = data.RfId;
                this.Number = data.Number.IsRequired(nameof(data.Number));
                this.MaxAverageSpeedInKmPh = data.MaxAverageSpeedInKmPh;
                this.HorseId = data.HorseId.IsRequired(nameof(data.HorseId));
                this.AthleteId = data.AthleteId.IsRequired(nameof(data.AthleteId));
            });
        }

        public string RfId { get; private set; }
        public int Number { get; private set; }
        public int? MaxAverageSpeedInKmPh { get; private set; }

        public int HorseId { get; private set; }

        public int AthleteId { get; private set; }
    }
}
