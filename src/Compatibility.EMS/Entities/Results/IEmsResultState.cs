using NTS.ACL.Abstractions;

namespace NTS.ACL.Entities.Results;

public interface IEmsResultState : IEmsIdentifiable
{
    bool IsNotQualified { get; }

    string Code { get; }
}
