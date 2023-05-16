using EMS.Core.Models;

namespace EMS.Core.Domain.State.Results;

public interface IResultState : IIdentifiable
{
    bool IsNotQualified { get; }

    string Code { get; }
}
