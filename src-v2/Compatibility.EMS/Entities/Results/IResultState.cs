using NTS.Compabitility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Results;

public interface IResultState : IIdentifiable
{
    bool IsNotQualified { get; }

    string Code { get; }
}
