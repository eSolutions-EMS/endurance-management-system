using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.Aggregates.Manager.ResultsInPhases
{
    public class ResultInPhase : DomainBase<ManagerResultInPhaseException>, IResultInPhaseState
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
