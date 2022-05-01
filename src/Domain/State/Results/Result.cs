using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.State.Results;

public class Result : DomainBase<ResultException>, IResultState
{
    private Result() {}
    internal Result(string code = null) : base(default)
    {
        this.Code = code;
    }

    public bool IsNotQualified => this.Code is not null;
    public string Code { get; private set; }
}
