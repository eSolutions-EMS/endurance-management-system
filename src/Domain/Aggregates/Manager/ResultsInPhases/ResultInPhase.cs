using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.States;

namespace EnduranceJudge.Domain.Aggregates.Manager.ResultsInPhases
{
    public class ResultInPhase : DomainObjectBase<ManagerResultInPhaseException>, IResultInPhaseState
    {
        internal ResultInPhase()
        {
            this.IsRanked = true;
        }

        internal ResultInPhase(string code)
        {
            this.Code = code;
            this.IsRanked = false;
        }

        public bool IsRanked { get; private set; }
        public string Code { get; private set; }
    }
}
