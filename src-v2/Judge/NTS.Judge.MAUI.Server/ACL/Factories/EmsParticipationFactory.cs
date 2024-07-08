using NTS.Domain.Core.Aggregates.Participations;
using NTS.Judge.MAUI.Server.ACL.Bridge;
using NTS.Judge.MAUI.Server.ACL.EMS;

namespace NTS.Judge.MAUI.Server.ACL.Factories;

public class EmsParticipationFactory
{
    public static EmsParticipation Create(Participation participation)
    {
        var athlete = EmsAthleteFactory.Create(participation);
        var horse = EmsHorseFactory.Create(participation);
        
        var state = new EmsParticipantState
        {
            Number = participation.Tandem.Number.ToString(),
            MaxAverageSpeedInKmPh = (int)participation.Tandem.MinAverageSpeed!,
            Unranked = true // TODO: fix when Unranked is added on Unranked level
        };
        var participant = new EmsParticipant(athlete, horse, state);
        var competition = EmsCompetitionFactory.Create(participation);

        return new EmsParticipation(participant, competition);
    }
}
