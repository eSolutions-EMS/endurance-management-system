using Core.Domain.Core.Exceptions;

namespace Core.Domain.State.Results;

public class ResultException : DomainExceptionBase
{
    protected override string Entity { get; } = nameof(Result);
}
