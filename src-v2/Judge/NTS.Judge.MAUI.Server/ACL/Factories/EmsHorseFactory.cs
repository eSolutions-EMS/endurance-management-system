using NTS.Compatibility.EMS.Entities.Horses;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Judge.MAUI.Server.ACL.Bridge;

namespace NTS.Judge.MAUI.Server.ACL.Factories;

public class EmsHorseFactory
{
    public static EmsHorse Create(Participation participation)
    {
        var state = new EmsHorseState
        {
            Name = participation.Tandem.Horse
        };
        return new EmsHorse(state);
    }
}
