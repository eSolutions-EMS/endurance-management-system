using Core.Domain.Common.Exceptions;
using Core.Domain.Common.Models;
using System.Text;

namespace Core.Domain.State.Results;

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
    public string TypeCode
    {
        get
        {
            switch (this.Type)
            {
                case ResultType.Resigned: return "RET";
                case ResultType.FailedToQualify: return "FTQ";
                case ResultType.Disqualified: return "DSQ";
                case ResultType.Successful: return "R";
                case ResultType.Invalid:
                default: throw Helper.Create<ResultException>("Invalid result type");
            }
        }
    }

    public override string ToString()
    {
        if (this.Type == ResultType.Successful)
        {
            return string.Empty;
        }
        var sb = new StringBuilder();
        sb.Append(TypeCode);
        sb.Append(" ");
        if (!string.IsNullOrEmpty(Code))
        {
            sb.Append(Code);
        }

        return sb.ToString();
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
