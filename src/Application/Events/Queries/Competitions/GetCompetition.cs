using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.Enums;
using System;

namespace EnduranceJudge.Application.Events.Queries.Competitions
{
    public class GetCompetition : IdentifiableRequest<CompetitionForUpdateModel>, ICompetitionState
    {
        public CompetitionType Type { get; }
        public string Name { get; }
        public DateTime StartTime { get; }

        public class GetCompetitionHandler : GetOneHandler<GetCompetition, CompetitionForUpdateModel, Competition>
        {
            public GetCompetitionHandler() : base()
            {
            }
        }
    }
}
