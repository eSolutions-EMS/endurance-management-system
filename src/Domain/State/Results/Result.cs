using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.State.Results
{
    public class Result : DomainBase<ResultException>, IResultState
    {
        private Result() {}
        internal Result(string code = null) : base(default)
        {
            this.IsRanked = code == null;
            this.Code = code;
        }

        public bool IsRanked { get; private set; }
        public string Code { get; private set; }
    }
}
