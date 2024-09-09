using Microsoft.JSInterop;
using Not.Blazor.Print;

namespace NTS.Judge.MAUI.Demo;

public class PrintInterop : IPrintInterop
{
    private readonly IJSRuntime _jsRuntime;

    public PrintInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task Print()
    {
        var module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/print-custom.js");
        await module.InvokeVoidAsync("printCustom");
    }
}
