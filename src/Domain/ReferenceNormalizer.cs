using EnduranceJudge.Domain.State;
using System.Linq;

namespace EnduranceJudge.Domain;

public class ReferenceNormalizer
{
    public static void Normalize(IState state)
    {
        foreach (var athlete in state.Athletes)
        {
            foreach (var participant in state.Participants)
            {
                if (participant.Athlete == athlete)
                {
                    participant.Athlete = athlete;
                }
            }
        }
        foreach (var horse in state.Horses)
        {
            foreach (var participant in state.Participants)
            {
                if (participant.Horse == horse)
                {
                    participant.Horse = horse;
                }
            }
        }
        foreach (var country in state.Countries)
        {
            if (state.Event.Country == country)
            {
                state.Event.Country = country;
            }
            foreach (var athlete in state.Athletes)
            {
                if (athlete.Country == country)
                {
                    athlete.Country = country;
                }
            }
        }
        // foreach (var phase in state.Event.Competitions.SelectMany(x => x.Phases))
        // {
        //     foreach (var participation in state.Participants.Select(x => x.Participation))
        //     {
        //         foreach (var performance in participation.Performances)
        //         {
        //             if (performance.Phase == phase)
        //             {
        //                 performance.Phase = phase;
        //             }
        //         }
        //     }
        // }
    }
}
