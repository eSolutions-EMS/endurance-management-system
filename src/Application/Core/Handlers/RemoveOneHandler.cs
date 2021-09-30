using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Domain.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Core.Handlers
{
    public class RemoveOneHandler<TRequest, TDomainModel> : Handler<TRequest>
        where TRequest : IdentifiableRequest
        where TDomainModel : IAggregateRoot
    {
        private readonly ICommands<TDomainModel> commands;

        public RemoveOneHandler(ICommands<TDomainModel> commands)
        {
            this.commands = commands;
        }

        public override async Task DoHandle(TRequest request, CancellationToken token)
        {
            await this.commands.Remove(request.Id, token);
        }
    }
}
