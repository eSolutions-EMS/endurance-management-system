using EnduranceJudge.Application.Contracts.Athletes;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Factories;
using EnduranceJudge.Application.Events.Models;
using EnduranceJudge.Application.Factories;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Events.Commands.UpdateAthlete
{
    public class UpdateAthlete : IRequest, IAthleteState
    {
        public int Id { get; set; }
        public string FeiId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Category Category { get; set; }
        public string CountryIsoCode { get; set; }

        public class UpdateAthleteHandler : Handler<UpdateAthlete>
        {
            private readonly IAthleteCommands athleteCommands;
            private readonly IAthleteFactory athleteFactory;

            public UpdateAthleteHandler(IAthleteCommands athleteCommands, IAthleteFactory athleteFactory)
            {
                this.athleteCommands = athleteCommands;
                this.athleteFactory = athleteFactory;
            }

            public override async Task DoHandle(UpdateAthlete request, CancellationToken token)
            {
                var athlete = this.athleteFactory.Create(request);
                await this.athleteCommands.Save<AthleteRootModel>(athlete, token);
            }
        }
    }
}
