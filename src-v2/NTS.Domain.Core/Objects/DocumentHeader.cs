using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects;

public record DocumentHeader : DomainObject
{
    public DocumentHeader(string @event, PopulatedPlace populatedPlace, EventSpan eventSpan, IEnumerable<Official> officials)
    {
        Title = @event;
        PopulatedPlace = populatedPlace;
        EventSpan = eventSpan;
        PresidentGroundJury = officials.FirstOrDefault(x => x.Role == OfficialRole.PresidentGroundJury);
        PresidentVet = officials.FirstOrDefault(x => x.Role == OfficialRole.PresidentVet);
        FeiTechDelegate = officials.FirstOrDefault(x => x.Role == OfficialRole.FeiTechDelegate);
        FeiVetDelegate = officials.FirstOrDefault(x => x.Role == OfficialRole.FeiDelegateVet);
    }

    public string Title { get; }
    public PopulatedPlace PopulatedPlace { get; }
    public EventSpan EventSpan { get; }
    public Official? PresidentGroundJury { get; }
    public Official? PresidentVet { get; }
    public Official? FeiTechDelegate { get; }
    public Official? FeiVetDelegate { get; }
}
