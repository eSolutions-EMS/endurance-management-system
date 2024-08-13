using Not.Events;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Events;
using NTS.Domain.Core.Objects;

namespace NTS.Domain.Core.Services;

public class DocumentProducer // TODO: remove
{
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
