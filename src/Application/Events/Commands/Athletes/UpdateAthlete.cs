using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Models;
using EnduranceJudge.Application.Factories;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using MediatR;
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
        public Category Category { get; set; }
        public string CountryIsoCode { get; set; }

        public class UpdateAthleteHandler : Handler<UpdateAthlete>
        {
            private readonly ICommands<Athlete> athleteCommands;
            private readonly IAthleteFactory athleteFactory;

            public UpdateAthleteHandler(ICommands<Athlete> athleteCommands, IAthleteFactory athleteFactory)
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
