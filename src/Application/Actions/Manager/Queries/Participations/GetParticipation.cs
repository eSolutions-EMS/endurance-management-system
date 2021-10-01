using EnduranceJudge.Application.Actions.Manager.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Actions.Manager.Queries.Participations
{
    public class GetParticipation : IRequest<Participation>
    {
        public int Number { get; set; }

        public class GetParticipationHandler : Handler<GetParticipation, Participation>
        {
            private readonly IParticipationQueries queries;
            public GetParticipationHandler(IParticipationQueries queries)
            {
                this.queries = queries;
            }

            public override async Task<Participation> Handle(GetParticipation request, CancellationToken token)
            {
                var participation = await this.queries.GetBy(request.Number);
                return participation;
            }
        }
    }
}
