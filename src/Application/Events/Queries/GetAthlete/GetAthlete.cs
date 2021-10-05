using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Application.Events.Models;
using EnduranceJudge.Domain.State.Athletes;

namespace EnduranceJudge.Application.Events.Queries.GetAthlete
{
    public class  GetAthlete : IdentifiableRequest<AthleteRootModel>
    {
        public class GetAthleteHandler : GetOneHandler<GetAthlete, AthleteRootModel, Athlete>
        {
            public GetAthleteHandler() : base()
            {
            }
        }
    }
}
