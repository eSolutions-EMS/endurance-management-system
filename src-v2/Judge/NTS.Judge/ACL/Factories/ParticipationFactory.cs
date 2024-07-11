using NTS.Compatibility.EMS.Entities.Participants;
using NTS.Domain.Core.Aggregates.Participations;
using NTS.Domain.Objects;
using NTS.Judge.ACL.Bridge;
using EmsParticipation = NTS.Compatibility.EMS.Entities.Participations.EmsParticipation;
using EmsCompetition = NTS.Compatibility.EMS.Entities.Competitions.EmsCompetition;
using EmsCompetitionType = NTS.Compatibility.EMS.Entities.Competitions.EmsCompetitionType;
using NTS.Domain.Enums;
using NTS.Domain;

namespace NTS.Judge.ACL.Factories;

public class ParticipationFactory
{
    public static EmsParticipation CreateEms(Participation participation)
    {
        var athlete = AthleteFactory.Create(participation);
        var horse = HorseFactory.Create(participation);
        
        var state = new EmsParticipantState
        {
            Number = participation.Tandem.Number.ToString(),
            MaxAverageSpeedInKmPh = (int)participation.Tandem.MinAverageSpeed!,
            Unranked = true // TODO: fix when Unranked is added on Unranked level
        };
        var participant = new EmsParticipant(athlete, horse, state);
        var competition = CompetitionFactory.Create(participation);

        return new EmsParticipation(participant, competition);
    }

    public static Participation CreateCore(EmsParticipation emsParticipation, EmsCompetition competition)
    {
        var tandem = new Tandem(
            int.Parse(emsParticipation.Participant.Number),
            new Person(emsParticipation.Participant.Athlete.Name),
            emsParticipation.Participant.Horse.Name,
            competition.Laps.Sum(x => (decimal)x.LengthInKm),
            null,
            null,
            12,
            emsParticipation.Participant.MaxAverageSpeedInKmPh);
        var phases = new List<Phase>();
        foreach (var record in emsParticipation.Participant.LapRecords)
        {
            var type = EmsCompetitionTypeToCompetitionType(competition.Type);
            var phase = new Phase(
                record.Lap.LengthInKm,
                record.Lap.MaxRecoveryTimeInMins,
                record.Lap.RestTimeInMins,
                type,
                record.Lap.IsFinal, 
                null);
            if (record.ArrivalTime.HasValue)
            {
                phase.ArriveTime = new Timestamp(record.ArrivalTime.Value);
            }
            if (record.InspectionTime.HasValue)
            {
                phase.InspectTime = new Timestamp(record.InspectionTime.Value);
            }
            if (record.ReInspectionTime.HasValue)
            {
                phase.ReinspectTime = new Timestamp(record.ReInspectionTime.Value);
            }
            phase.StartTime = new Timestamp(record.StartTime);
            phase.IsReinspectionRequested = record.IsRequiredInspectionRequired;
            phase.IsCRIRequested = record.IsRequiredInspectionRequired;
            
            phases.Add(phase);
        }
        return new Participation(competition.Name, tandem, phases);
    }

    private static CompetitionType EmsCompetitionTypeToCompetitionType(EmsCompetitionType emsCompetitionType)
    {
        return emsCompetitionType switch
        {
            EmsCompetitionType.National => CompetitionType.National,
            EmsCompetitionType.International => CompetitionType.FEI,
            _ => throw new NotImplementedException(),
        };
    }
}
