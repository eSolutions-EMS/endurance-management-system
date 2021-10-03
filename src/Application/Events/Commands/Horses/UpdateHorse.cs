using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Domain.State.Horses;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Events.Commands.Horses
{
    public class UpdateHorse : IRequest, IHorseState
    {
        public int Id { get; set; }
        public string FeiId { get; set; }
        public string Name { get; set; }
        public string Club { get; set; }
        public bool IsStallion { get; set; }
        public string Breed { get; set; }
        public string TrainerFeiId { get; set; }
        public string TrainerFirstName { get; set; }
        public string TrainerLastName { get; set; }

        public class UpdateHorseHandler : Handler<UpdateHorse>
        {
            public UpdateHorseHandler()
            {
            }

            public override async Task DoHandle(UpdateHorse request, CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}
