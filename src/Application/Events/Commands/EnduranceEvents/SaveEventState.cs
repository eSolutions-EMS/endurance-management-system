using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.States;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Events.Commands.EnduranceEvents
{
    public class SaveEventState : IRequest, IEventState
    {
        public int Id { get; set;  }
        public string Name { get; set; }
        public string PopulatedPlace { get; set; }
        public string CountryIsoCode { get; set; }
        public IEnumerable<PersonnelDependantModel> Personnel { get; set; }
        public IEnumerable<CompetitionDependantModel> Competitions { get; set;}

        public class SaveEventStateHandler : Handler<SaveEventState>
        {
            public SaveEventStateHandler()
            {
            }

            public override async Task DoHandle(SaveEventState request, CancellationToken token)
            {
            }
        }
    }
}
