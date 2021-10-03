using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Events.Commands.Athletes
{
    public class UpdateAthlete : IRequest, IAthleteState
    {
        public int Id { get; set; }
        public string FeiId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Club { get; set; }
        public Category Category { get; set; }
        public string CountryIsoCode { get; set; }

        public class UpdateAthleteHandler : Handler<UpdateAthlete>
        {

            public UpdateAthleteHandler()
            {
            }

            public override async Task DoHandle(UpdateAthlete request, CancellationToken token)
            {
                throw new NotImplementedException();
            }
        }
    }
}
