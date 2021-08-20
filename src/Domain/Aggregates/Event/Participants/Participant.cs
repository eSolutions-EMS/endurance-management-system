using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.Aggregates.Event.Participants
{
    public class Participant : DomainBase<ParticipantException>, IParticipantState
    {
        private Participant() : base(default)
        {
        }

        public Participant(IParticipantState data) : base(data.Id)
            => this.Validate(() =>
            {
                this.RfId = data.RfId.IsRequired(nameof(data.RfId));
                this.Number = data.Number.IsRequired(nameof(data.Number));
                this.MaxAverageSpeedInKmPh = data.MaxAverageSpeedInKmPh;
                this.HorseId = data.HorseId.IsRequired(nameof(data.HorseId));
                this.AthleteId = data.AthleteId.IsRequired(nameof(data.AthleteId));
            });

        public string RfId { get; private set; }
        public int Number { get; private set; }
        public int? MaxAverageSpeedInKmPh { get; private set; }

        public int HorseId { get; private set; }

        public int AthleteId { get; private set; }
    }
}
