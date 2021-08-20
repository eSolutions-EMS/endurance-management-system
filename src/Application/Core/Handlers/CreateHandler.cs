// using EnduranceJudge.Application.Core.Factories;
// using EnduranceJudge.Application.Core.Contracts;
// using EnduranceJudge.Domain.Core.Models;
// using MediatR;
// using System.Threading;
// using System.Threading.Tasks;
//
// namespace EnduranceJudge.Application.Core.Handlers
// {
//     public abstract class CreateHandler<TRequest, TEntity> : Handler<TRequest, int>
//         where TRequest : IRequest<int>, IDomainModelState
//         where TEntity : IAggregateRoot
//     {
//         private readonly IFactory<TEntity> factory;
//         private readonly ICommandsBase<TEntity> commands;
//
//         protected CreateHandler(IFactory<TEntity> factory, ICommandsBase<TEntity> commands)
//         {
//             this.factory = factory;
//             this.commands = commands;
//         }
//
//         public override async Task<int> Handle(TRequest request, CancellationToken cancellationToken)
//         {
//             var entity = this.factory.Create(request);
//
//             await this.commands.Save(entity, cancellationToken);
//
//             return entity.Id;
//         }
//     }
// }
