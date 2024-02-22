using Common.Conventions;
using Common.Domain;
using Common.Services;
using Common.Utilities;
using MudBlazor;
using Not.Blazor.Dialogs.Components;
using Not.Blazor.Forms;

namespace Not.Blazor.Dialogs;

public class NotDialogService<T, TForm> : INotDialogService<T, TForm>
    where T : DomainEntity
    where TForm : NotFormFields<T>
{
    private readonly IDialogService _mudDialogService;
    private readonly ILocalizer _localizer;

    public NotDialogService(IDialogService mudDialogService, ILocalizer localizer)
    {
        _mudDialogService = mudDialogService;
        _localizer = localizer;
    }

    public async Task RenderCreate()
    {
        var title = _localizer.Get("Create", " ", ReflectionHelper.GetName<T>());
        var dialog = await _mudDialogService.ShowAsync<CreateDialog<T, TForm>>(title);
        await dialog.Result;
    }
}

public interface INotDialogService<T, TForm> : ITransientService
    where T : DomainEntity
    where TForm : NotFormFields<T>
{
    Task RenderCreate();
}
