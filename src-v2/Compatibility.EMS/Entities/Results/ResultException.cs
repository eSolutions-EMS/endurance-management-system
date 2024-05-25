using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Results;

public class ResultException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Result);
}
