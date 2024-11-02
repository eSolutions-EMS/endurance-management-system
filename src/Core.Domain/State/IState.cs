using System.Collections.Generic;
using Core.Domain.State.Athletes;
using Core.Domain.State.Countries;
using Core.Domain.State.EnduranceEvents;
using Core.Domain.State.Horses;
using Core.Domain.State.Participants;
using Core.Domain.State.Participations;

namespace Core.Domain.State;

public interface IState
{
    EnduranceEvent Event { get; set; }
    List<Horse> Horses { get; }
    List<Athlete> Athletes { get; }
    List<Participant> Participants { get; }
    List<Participation> Participations { get; }
    IReadOnlyList<Country> Countries { get; }
}
