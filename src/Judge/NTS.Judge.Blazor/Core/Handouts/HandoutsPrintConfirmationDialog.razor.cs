using MudBlazor;

namespace NTS.Judge.Blazor.Core.Handouts;

public partial class HandoutsPrintConfirmationDialog
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; } = default!;

    void Confirm()
    {
        var dialogResult = DialogResult.Ok(true);
        MudDialog.Close(dialogResult);
    }

    void Cancel()
    {
        MudDialog.Cancel();
    }
}
