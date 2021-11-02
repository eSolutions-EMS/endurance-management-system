using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.State.PhaseResults
{
    public class PhaseResult : DomainObjectBase<PhaseResultException>, IPhaseResultState
    {
        private PhaseResult() {}
        internal PhaseResult(string code = null) : base(default)
        {
            this.IsRanked = code == null;
            this.Code = code;
        }

        public bool IsRanked { get; private set; }
        public string Code { get; private set; }
    }
}
