using EnduranceJudge.Domain.Core.Models;
using System;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Domain.State.Results;

public class Result : DomainBase<ResultException>, IResultState
{
    private Result() {}
    internal Result(ResultType type, string code = null) : base(default)
    {
        this.Code = code;
        this.Type = type;
    }

    public bool IsNotQualified => this.Type != ResultType.Successful;
    public string Code { get; private set; }
    public ResultType Type { get; private set; } = ResultType.Successful;

    public override string ToString()
    {
        if (this.Type == ResultType.Successful)
        {
            return string.Empty;
        }
        var typeString = string.Empty;
        switch (this.Type)
        {
            case ResultType.Resigned: typeString = RET; break;
            case ResultType.FailedToQualify: typeString = FTQ; break;
            case ResultType.Disqualified: typeString = DQ; break;
            case ResultType.Successful:
            case ResultType.Invalid:
            default: break;
        }

        return $"{typeString.ToUpper()} {this.Code}";
    }
}

public enum ResultType
{
    Invalid = 0,
    Resigned = 1,
    FailedToQualify = 2,
    Disqualified = 3,
    Successful = 4
}
