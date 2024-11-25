using Not.Blazor.Navigation;

namespace NTS.Judge.Blazor.Setup.EnduranceEvents.Contestants;

public partial class ParticipationUpdate
{
    [Inject]
    ICrumbsNavigator Navigator { get; set; } = default!;
}
