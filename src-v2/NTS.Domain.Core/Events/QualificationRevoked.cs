﻿using Not.Events;
using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Domain.Core.Events;

public record QualificationRevoked : DomainObject, IEvent
{
    public QualificationRevoked(int number, NotQualified notQualified)
    {
        Number = number;
        NotQualified = notQualified;
    }

    public int Number { get; }
    public NotQualified NotQualified { get; }
}