using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Actions.Manager.Commands.UpdateParticipation
{
    public class UpdateParticipation : IdentifiableRequest, IMapTo<Participation>
    {
        public UpdateParticipation(Participation participation)
        {
            this.Participation = participation;
        }

        public Participation Participation { get; }

        public class UpdateParticipationHandler : Handler<UpdateParticipation>
        {
            public UpdateParticipationHandler()
            {
            }

            public override async Task DoHandle(UpdateParticipation request, CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}
