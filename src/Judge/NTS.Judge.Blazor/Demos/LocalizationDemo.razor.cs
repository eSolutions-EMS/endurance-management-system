using System.Globalization;

namespace NTS.Judge.Blazor.Demos;

public partial class LocalizationDemo
{
    string _polite = "";

    protected async Task ChangeCulture(string name)
    {
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(name);
        await InvokeAsync(StateHasChanged);
    }

    protected async Task SayPolite()
    {
        _polite = StringLocalizer[LocalizationTestService.Polite()];
        await InvokeAsync(StateHasChanged);
    }
}
