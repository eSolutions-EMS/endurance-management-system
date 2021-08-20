using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Application.Events.Models;
using EnduranceJudge.Domain.Aggregates.Common.Horses;

namespace EnduranceJudge.Application.Events.Queries.GetHorse
{
    public class GetHorse : IdentifiableRequest<HorseRootModel>
    {
        public class GetHorseHandler : GetOneHandler<GetHorse, HorseRootModel, Horse>
        {
            public GetHorseHandler(IQueriesBase<Horse> query) : base(query)
            {
            }
        }
    }
}
