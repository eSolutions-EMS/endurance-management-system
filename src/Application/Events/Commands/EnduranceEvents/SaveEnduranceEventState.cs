using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.State.EnduranceEvents;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Events.Commands.EnduranceEvents
{
    public class SaveEnduranceEventState : IRequest, IEnduranceEventState
    {
        public int Id { get; set;  }
        public string Name { get; set; }
        public string PopulatedPlace { get; set; }
        public string CountryIsoCode { get; set; }
        public IEnumerable<PersonnelDependantModel> Personnel { get; set; }
        public IEnumerable<CompetitionDependantModel> Competitions { get; set;}

        public class SaveEventStateHandler : Handler<SaveEnduranceEventState>
        {
            public SaveEventStateHandler()
            {
            }

            public override async Task DoHandle(SaveEnduranceEventState request, CancellationToken token)
            {
            }
        }
    }
}
