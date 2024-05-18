using Not.Events;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Events;
using NTS.Domain.Core.Objects;

namespace NTS.Domain.Core.Services;

public class DocumentProducer
{
    public static void CreateHandout(Event @event, IEnumerable<Official> officials, Participation participation)
    {
        var header = new DocumentHeader(participation.Competition, @event.PopulatedPlace, @event.EventSpan, officials);
        var handoutProduced = new HandoutDocumentProduced(header, participation.Tandem, participation.Phases);
        EventHelper.Emit(handoutProduced);
    }

    public static void CreateRanklist(Event @event, IEnumerable<Official> officials, Classification classification)
    {
        var header = new DocumentHeader(classification.Name, @event.PopulatedPlace, @event.EventSpan, officials);
        var ranklist = new Ranklist(classification);
        var ranklistProduced = new RanklistDocumentProduced(header, ranklist);
        EventHelper.Emit(ranklistProduced);
    }
}
