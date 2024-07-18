using NTS.Compatibility.EMS.Abstractions;

namespace NTS.Compatibility.EMS.Entities.Results;

public interface IEmsResultState : IEmsIdentifiable
{
    bool IsNotQualified { get; }

    string Code { get; }
}
