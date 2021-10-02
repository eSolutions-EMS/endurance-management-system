using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Actions.Manager.Commands
{
    public class StartParticipations : IRequest
    {
        public class StartParticipationsHandler : Handler<StartParticipations>
        {
            private readonly ICommands<Participation> commands;
            public StartParticipationsHandler(ICommands<Participation> commands)
            {
                this.commands = commands;
            }

            public override async Task DoHandle(StartParticipations request, CancellationToken token)
            {
                var participations = await this.commands.All();
                foreach (var participation in participations)
                {
                    participation.Start();
                }

                await this.commands.Update(participations, token);
            }
        }
    }
}
