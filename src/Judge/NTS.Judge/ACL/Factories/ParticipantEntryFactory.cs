﻿using NTS.Compatibility.EMS.Entities;
using NTS.Domain.Core.Aggregates;

namespace NTS.Judge.ACL.Factories;

public class ParticipantEntryFactory
{
    public static EmsParticipantEntry Create(Participation participation)
    {
        var emsParticipation = ParticipationFactory.CreateEms(participation);
        return new EmsParticipantEntry(emsParticipation);
    }
}