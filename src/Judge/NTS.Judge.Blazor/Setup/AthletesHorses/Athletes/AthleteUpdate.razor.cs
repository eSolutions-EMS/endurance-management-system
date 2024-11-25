using Not.Blazor.Navigation;

namespace NTS.Judge.Blazor.Setup.AthletesHorses.Athletes;

public partial class AthleteUpdate
{
    [Inject]
    ICrumbsNavigator Navigator { get; set; } = default!;
}
