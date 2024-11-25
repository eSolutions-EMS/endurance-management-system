using Not.Blazor.Navigation;
using static NTS.Judge.Blazor.Shared.Constants.Endpoints;

namespace NTS.Judge.Blazor.Shared.Components.Nav;

public partial class NavMenu
{
    [Inject]
    ILandNavigator LandNavigator { get; set; } = default!;

    protected override void OnInitialized()
    {
        LandNavigator.Initialize(ENDURANCE_EVENT_PAGE);
    }
}
