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
        var handout = new HandoutDocument(header, participation.Tandem, participation.Phases);
        Produce(handout);
    }

    public static void CreateRanklist(Event @event, IEnumerable<Official> officials, Ranking classification)
    {
        var ranklist = new Ranklist(classification);

        var header = new DocumentHeader(classification.Name, @event.PopulatedPlace, @event.EventSpan, officials);
        var ranklistDocument = new RanklistDocument(header, ranklist);
        Produce(ranklistDocument);
    }

    private static void Produce<T>(T document)
        where T : Document
    {
        var documentProduced = new DocumentProduced(document);
        EventHelper.Emit(documentProduced);
    }
}
