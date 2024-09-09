using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects;

public record DocumentHeader : DomainObject
{
    private DocumentHeader()
    {
    }
    public DocumentHeader(string @event, PopulatedPlace populatedPlace, EventSpan eventSpan, IEnumerable<Official> officials)
    {
        Title = @event;
        PopulatedPlace = populatedPlace;
        EventSpan = eventSpan;
        PresidentGroundJury = officials.FirstOrDefault(x => x.Role == OfficialRole.PresidentGroundJury);
        PresidentVet = officials.FirstOrDefault(x => x.Role == OfficialRole.PresidentVet);
        FeiTechDelegate = officials.FirstOrDefault(x => x.Role == OfficialRole.FeiTechDelegate);
        FeiVetDelegate = officials.FirstOrDefault(x => x.Role == OfficialRole.FeiVetDelegate);
        ActiveVet = officials.FirstOrDefault(x => x.Role == OfficialRole.ActiveVet);
        ForeignJudge = officials.FirstOrDefault(x => x.Role == OfficialRole.ForeignJudge);
    }

    public string Title { get; private set; }
    public PopulatedPlace PopulatedPlace { get; private set; }
    public EventSpan EventSpan { get; private set; }
    public Official? PresidentGroundJury { get; private set; }
    public Official? PresidentVet { get;  private set; }
    public Official? FeiTechDelegate { get; private set; }
    public Official? FeiVetDelegate { get; private set; }
    public Official? ActiveVet { get; private set; }
    public Official? ForeignJudge { get; private set; }
}
