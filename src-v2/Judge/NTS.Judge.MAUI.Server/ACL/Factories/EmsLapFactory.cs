using NTS.Compatibility.EMS.Entities.Laps;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Judge.MAUI.Server.ACL.Bridge;

namespace NTS.Judge.MAUI.Server.ACL.Factories;

public class EmsLapFactory
{
    public static IEnumerable<EmsLap> Create(Participation participation)
    {
        var i = 0;
        foreach (var phase in participation.Phases)
        {
            var state = new EmsLapState
            {
                Id = phase.Id,
                IsFinal = participation.Phases.Last() == phase ? true : false,
                IsCompulsoryInspectionRequired = phase.IsCRIRequested,
                LengthInKm = phase.Length,
                MaxRecoveryTimeInMins = phase.MaxRecovery,
                OrderBy = ++i,
                RestTimeInMins = phase.Rest
            };
            yield return new EmsLap(state);
        }
    }
}
