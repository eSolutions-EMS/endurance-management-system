using NTS.Compatibility.EMS.Entities.Competitions;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Judge.MAUI.Server.ACL.Bridge;

namespace NTS.Judge.MAUI.Server.ACL.Factories;

public class EmsCompetitionFactory
{
    public static EmsCompetition Create(Participation participation)
    {
        var laps = EmsLapFactory.Create(participation);
        var state = new EmsCompetitionState
        {
            Id = participation.Id,
            Name = participation.Competition,
            Type = EmsCompetitionType.International //TODO: probably has to change as we will probably have to add Type to Participation in NTS
        };
        var competition = new EmsCompetition(state);
        foreach (var lap in laps)
        {
            competition.Save(lap);
        }
        return competition;
    }
}
