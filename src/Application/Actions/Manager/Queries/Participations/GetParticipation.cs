using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Actions.Manager.Queries.Participations
{
    public class GetParticipation : IRequest<Participation>
    {
        public int Number { get; set; }

        public class GetParticipationHandler : Handler<GetParticipation, Participation>
        {
            public GetParticipationHandler()
            {
            }

            public override async Task<Participation> Handle(GetParticipation request, CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}
