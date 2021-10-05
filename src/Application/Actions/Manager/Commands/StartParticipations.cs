using EnduranceJudge.Application.Core.Handlers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Actions.Manager.Commands
{
    public class StartParticipations : IRequest
    {
        public class StartParticipationsHandler : Handler<StartParticipations>
        {
            public StartParticipationsHandler()
            {
            }

            public override async Task DoHandle(StartParticipations request, CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}
