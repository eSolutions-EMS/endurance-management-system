using MudBlazor;

namespace NTS.Judge.Blazor.Core.Handouts;
public partial class HandoutsPage
{
    [Inject]
    IHandoutsBehind Behind { get; set; } = default!;
    [Inject]
    IDialogService DialogService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await Observe(Behind);
    }

    async Task OpenPrintPreview()
    {
        var handouts = Behind.Documents.ToList();

        await OpenPrintDialog();
        var dialog = await DialogService.ShowAsync<HandoutsPrintConfirmationDialog>();
        var result = await dialog.Result;
        if (!result?.Canceled ?? false)
        {
            await Behind.Delete(handouts);
        }
    }
}
