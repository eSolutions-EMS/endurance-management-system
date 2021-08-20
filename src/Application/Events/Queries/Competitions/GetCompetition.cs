using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Domain.Aggregates.Event.Competitions;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Application.Events.Queries.Competitions
{
    public class GetCompetition : IdentifiableRequest<CompetitionForUpdateModel>, ICompetitionState
    {
        public CompetitionType Type { get; }
        public string Name { get; }

        public class GetCompetitionHandler : GetOneHandler<GetCompetition, CompetitionForUpdateModel, Competition>
        {
            public GetCompetitionHandler(IQueriesBase<Competition> query) : base(query)
            {
            }
        }
    }
}
