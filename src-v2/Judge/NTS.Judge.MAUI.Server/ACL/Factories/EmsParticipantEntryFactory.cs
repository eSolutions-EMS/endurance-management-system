using NTS.Domain.Core.Aggregates.Participations;
using NTS.Judge.MAUI.Server.ACL.EMS;

namespace NTS.Judge.MAUI.Server.ACL.Factories;

public class EmsParticipantEntryFactory
{
    public static EmsParticipantEntry Create(Participation participation)
    {
        var emsParticipation = EmsParticipationFactory.Create(participation);
        return new EmsParticipantEntry(emsParticipation);
    }
}
