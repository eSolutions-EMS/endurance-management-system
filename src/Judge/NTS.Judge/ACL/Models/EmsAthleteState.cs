using Core.Domain.State.Athletes;
using NTS.Compatibility.EMS.Enums;

namespace NTS.Judge.ACL.Bridge;

internal class EmsAthleteState : IEmsAthleteState
{
    public string? FeiId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Club { get; set; }
    public EmsCategory Category { get; set; }
    public int Id { get; set; }
}
