using Not.Events;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Events;

namespace NTS.Domain.Core.Services;

public class Startlist
{
    public static void CreateStart(Participation participation)
    {
        if (participation.IsNotQualified)
        {
            return;
        }
        if (participation.Phases.OutTime == null)
        {
            return;
        }

        var starter = new StartCreated(
            participation.Tandem.Number,
            participation.Tandem.Name,
            participation.Phases.CurrentNumber,
            participation.Phases.Distance,
            participation.Phases.OutTime.Value);

        EventHelper.Emit(starter);
    }
}
