﻿using NTS.ACL.Entities.Laps;
using NTS.ACL.Models;
using NTS.Domain.Core.Aggregates;

namespace NTS.ACL.Factories;

public class LapFactory
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
                IsCompulsoryInspectionRequired = phase.IsRequiredInspectionCompulsory,
                LengthInKm = phase.Length,
                MaxRecoveryTimeInMins = phase.MaxRecovery,
                OrderBy = ++i,
                RestTimeInMins = phase.Rest ?? 0,
            };
            yield return new EmsLap(state);
        }
    }
}
