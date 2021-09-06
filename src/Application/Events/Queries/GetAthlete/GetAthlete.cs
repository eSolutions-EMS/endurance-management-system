using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Application.Events.Models;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;

namespace EnduranceJudge.Application.Events.Queries.GetAthlete
{
    public class  GetAthlete : IdentifiableRequest<AthleteRootModel>
    {
        public class GetAthleteHandler : GetOneHandler<GetAthlete, AthleteRootModel, Athlete>
        {
            public GetAthleteHandler(IQueriesBase<Athlete> query) : base(query)
            {
            }
        }
    }
}
