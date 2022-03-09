using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using System.Collections.Generic;

namespace EnduranceJudge.Domain.State
{
    public interface IState
    {
        EnduranceEvent Event { get; set; }
        List<Horse> Horses { get; }
        List<Athlete> Athletes { get; }
        List<Participant> Participants { get; }
        List<Participation> Participations { get; }
        IReadOnlyList<Country> Countries { get; }
    }
}
