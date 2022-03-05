using System;

namespace EnduranceJudge.Domain.AggregateRoots;

public interface ICompetitionData
{
    public string EventName { get; }
    public string PopulatedPlace { get; }
    public string CountryName { get; }
    public string Organizer { get; } // ?
    public string ChiefStewardName { get; }
    public DateTime DateNow { get; }
    public string CompetitionName { get; }
    public DateTime CompetitionDate { get; }
    public double CompetitionLengthInKm { get; }
    public string PresidentGroundJuryName { get; }
}
