using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Factories;
using EnduranceJudge.Application.Import.Contracts;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Events.Commands
{
    public class UpdateHorse : IRequest, IHorseState
    {
        public int Id { get; set; }
        public string FeiId { get; set; }
        public string Name { get; set; }
        public bool IsStallion { get; set; }
        public string Breed { get; set; }
        public string TrainerFeiId { get; set; }
        public string TrainerFirstName { get; set; }
        public string TrainerLastName { get; set; }

        public class UpdateHorseHandler : Handler<UpdateHorse>
        {
            private readonly IHorseCommands horseCommands;
            private readonly IHorseFactory horseFactory;

            public UpdateHorseHandler(IHorseCommands horseCommands, IHorseFactory horseFactory)
            {
                this.horseCommands = horseCommands;
                this.horseFactory = horseFactory;
            }

            public override async Task DoHandle(UpdateHorse request, CancellationToken token)
            {
                var horse = this.horseFactory.Create(request);
                await this.horseCommands.Save(horse, token);
            }
        }
    }
}
