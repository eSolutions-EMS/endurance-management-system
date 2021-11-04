using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participations;

namespace EnduranceJudge.Domain.State.Participants
{
    public class Participant : DomainObjectBase<ParticipantException>, IParticipantState
    {
        public const int DEFAULT_MAX_AVERAGE_SPEED = 16;
        public const string NAME_FORMAT = "{0} - {1}";

        private Participant() {}
        public Participant(Athlete athlete, Horse horse) : base(GENERATE_ID)
        {
            this.Horse = horse;
            this.Athlete = athlete;
        }
        public Participant(Athlete athlete, Horse horse, IParticipantState state) : base(GENERATE_ID)
        {
            this.RfId = state.RfId;
            this.MaxAverageSpeedInKmPh = state.MaxAverageSpeedInKmPh;
            this.Number = state.Number;
            this.Athlete = athlete;
            this.Horse = horse;
        }

        public string RfId { get; internal set; }
        public int Number { get; internal set; }
        public int? MaxAverageSpeedInKmPh { get; internal set; }
        public Horse Horse { get; internal set; }
        public Athlete Athlete { get; internal set; }
        public Participation Participation { get; private set; } = new();

        public void ParticipateIn(Competition competition)
        {
            this.Participation.Add(competition);
        }

        public string Name => string.Format(NAME_FORMAT, this.Athlete.Name, this.Horse.Name);
    }
}
