using EMS.Core.Domain.State.Athletes;
using EMS.Core.Domain.State.Countries;
using EMS.Core.Domain.State.EnduranceEvents;
using EMS.Core.Domain.State.Horses;
using EMS.Core.Domain.State.Participants;
using EMS.Core.Domain.State.Participations;
using System.Collections.Generic;

namespace EMS.Core.Domain.State;

public interface IState
{
    EnduranceEvent Event { get; set; }
    List<Horse> Horses { get; }
    List<Athlete> Athletes { get; }
    List<Participant> Participants { get; }
    List<Participation> Participations { get; }
    IReadOnlyList<Country> Countries { get; }
}
