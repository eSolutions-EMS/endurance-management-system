using Not.Notify;
using NTS.Domain.Core.Aggregates.Participations;

namespace NTS.Judge.Blazor.Core.Dashboards.Actions.Eliminations.EliminationForms;
public partial class DisqualifyForm
{
    string? Reason { get; set; }

    [Parameter]
    public Disqualified? Disqualified { get; set; }

    protected override void OnParametersSet()
    {
        Reason = Disqualified?.Complement;
    }

    internal override async Task Eliminate()
    {
        if (Reason == null)
        {
            NotifyHelper.Warn("Reason is required");
            return;
        }
        await Eliminations.Disqualify(Reason);
    }
}
