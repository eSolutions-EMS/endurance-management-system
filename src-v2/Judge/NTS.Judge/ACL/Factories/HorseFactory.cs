using NTS.Compatibility.EMS.Entities.Horses;
using NTS.Domain.Core.Aggregates;
using NTS.Judge.ACL.Bridge;

namespace NTS.Judge.ACL.Factories;

public class HorseFactory
{
    public static EmsHorse Create(Participation participation)
    {
        var state = new EmsHorseState { Name = participation.Combination.Horse };
        return new EmsHorse(state);
    }
}
