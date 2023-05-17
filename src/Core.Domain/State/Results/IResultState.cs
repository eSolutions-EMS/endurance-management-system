using Core.Models;

namespace Core.Domain.State.Results;

public interface IResultState : IIdentifiable
{
    bool IsNotQualified { get; }

    string Code { get; }
}
