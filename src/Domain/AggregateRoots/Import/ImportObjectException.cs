using EnduranceJudge.Domain.Core.Exceptions;

namespace EnduranceJudge.Domain.Aggregates.Import
{
    public class ImportObjectException : DomainExceptionBase
    {
        protected override string Entity { get; } = nameof(ImportRoot);
    }
}
