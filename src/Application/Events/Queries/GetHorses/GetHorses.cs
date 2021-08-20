using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using MediatR;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Events.Queries.GetHorses
{
    public class GetHorses : IRequest<IEnumerable<HorseModel>>
    {
        public class GetHorsesHandler : GetAllHandler<GetHorses, HorseModel, Horse>
        {
            public GetHorsesHandler(IQueriesBase<Horse> horseQueries) : base(horseQueries)
            {
            }
        }
    }
}
