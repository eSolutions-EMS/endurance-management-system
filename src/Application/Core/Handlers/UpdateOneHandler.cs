using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Core.Handlers
{
    public class UpdateOneHandler<TRequest, TDomainModel> : Handler<TRequest>
        where TRequest : IdentifiableRequest, IMapTo<TDomainModel>
        where TDomainModel : IAggregateRoot
    {
        private readonly ICommands<TDomainModel> commands;

        protected UpdateOneHandler(ICommands<TDomainModel> commands)
        {
            this.commands = commands;
        }

        public override async Task DoHandle(TRequest request, CancellationToken token)
        {
            var entity = await this.commands.Find<TDomainModel>(request.Id);
            this.Update(entity, request);
            await this.commands.Save(entity, token);
        }

        protected virtual void Update(TDomainModel entity, TRequest request)
        {
            var update = request.Map<TDomainModel>();
            entity.MapFrom(update);
        }
    }
}
