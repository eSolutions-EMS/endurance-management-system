using System.Text;

namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsResult : EmsDomainBase<EmsResultException>, IEmsResultState
{
    private EmsResult() { }
    internal EmsResult(ResultType type, string code = null) : base(default)
    {
        Code = code;
        Type = type;
    }

    public bool IsNotQualified => Type != ResultType.Successful;
    public string Code { get; private set; }
    public ResultType Type { get; private set; } = ResultType.Successful;
    public string TypeCode
    {
        get
        {
            switch (Type)
            {
                case ResultType.Resigned: return "RET";
                case ResultType.FailedToQualify: return "FTQ";
                case ResultType.Disqualified: return "DSQ";
                case ResultType.Successful: return "R";
                case ResultType.Invalid:
                default: throw EmsHelper.Create<EmsResultException>("Invalid result type");
            }
        }
    }

    public override string ToString()
    {
        if (Type == ResultType.Successful)
        {
            return string.Empty;
        }
        var sb = new StringBuilder();
        sb.Append(TypeCode);
        sb.Append(" ");
        if (!string.IsNullOrEmpty(Code) && TypeCode != "RET")
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
