using NTS.Judge.MAUI.Server.ACL.EMS;

namespace NTS.Judge.MAUI.Server.ACL.Bridge;

internal class EmsAthleteState : IEmsAthleteState
{
    public string? FeiId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Club { get; set; }
    public EmsCategory Category { get; set; }
    public int Id { get; set; }
}
