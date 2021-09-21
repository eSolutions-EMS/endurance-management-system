using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;

namespace EnduranceJudge.Application.Actions.Manager.Queries.GetParticipation
{
    public class GetParticipation : IdentifiableRequest<Participation>
    {
        public class GetParticipationHandler : GetOneHandler<GetParticipation, Participation, Participation>
        {
            public GetParticipationHandler(IQueries<Participation> query) : base(query)
            {
            }
        }
    }
}
