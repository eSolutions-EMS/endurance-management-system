using System.Collections.Generic;

namespace EnduranceJudge.Domain.AggregateRoots.Import.Models.International;

public class InternationalData
{
    public HorseSportShowEntriesShowVenue Event { get; init; }
    public List<HorseSportShowEntriesEvent> Competitions { get; init; } = new();
    public List<HorseSportShowEntriesHorse> Horses { get; init; } = new();
    public List<HorseSportShowEntriesAthlete> Athletes { get; init; } = new();
    public List<HorseSportShowEntriesEventAthleteEntry> Participants { get; init; } = new();
}