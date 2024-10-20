using NTS.Domain.Core.Entities.ParticipationAggregate;
﻿using Not;
using NTS.Domain.Core.Entities;

namespace NTS.Domain.Core.Objects;

public record HandoutDocument : DomainObject, IIdentifiable
{
    public HandoutDocument(Participation participation, EnduranceEvent enduranceEvent, IEnumerable<Official> officials)
    {
        Id = participation.Id;
        Header = new DocumentHeader(participation.Competition.Name, enduranceEvent.PopulatedPlace, enduranceEvent.EventSpan, officials);
        Tandem = participation.Tandem;
        Phases = participation.Phases;
    }

    public int Id { get; }
    public DocumentHeader Header { get; }
    public Tandem Tandem { get; }
    public PhaseCollection Phases { get; }
}
