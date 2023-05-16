using EMS.Core.Domain.Core.Exceptions;

namespace EMS.Core.Domain.State.Results;

public class ResultException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Result);
}
