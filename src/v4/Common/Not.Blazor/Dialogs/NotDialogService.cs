using Common.Conventions;
using Common.Domain;
using Common.Services;
using Common.Utilities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Not.Blazor.Dialogs.Components;
using Not.Blazor.Forms;

namespace Not.Blazor.Dialogs;

public class NotDialogService<T, TModel, TForm> : INotDialogService<T, TModel, TForm>
    where T : DomainEntity
    where TModel : class, new()
    where TForm : FormContent
{
    private readonly IDialogService _mudDialogService;
    private readonly ILocalizer _localizer;

    public NotDialogService(IDialogService mudDialogService, ILocalizer localizer)
    {
        _mudDialogService = mudDialogService;
        _localizer = localizer;
    }

    public async Task CreateEntity(Func<TModel, T> factory)
    {
        await Show<CreateDialog<T, TModel, TForm>>(factory);
    }

    public async Task CreateChildEntity(Func<TModel, T> factory)
    {
        await Show<CreateChildDialog<T, TModel, TForm>>(factory);
    }

    private async Task Show<TDialog>(Func<TModel, T> factory)
        where TDialog : ComponentBase, INotCreateDialog<T, TModel>
    {
        var parameters = new DialogParameters<TDialog>
        {   
            { x => x.Factory, factory }
        };

        var title = _localizer.Get("Create", " ", ReflectionHelper.GetName<T>());
        var dialog = await _mudDialogService.ShowAsync<TDialog>(title, parameters);
        await dialog.Result;
    }
}

public interface INotDialogService<T, TModel, TForm> : ITransientService
    where T : DomainEntity
    where TModel : class, new()
    where TForm : FormContent
{
    Task CreateEntity(Func<TModel, T> factory);
    Task CreateChildEntity(Func<TModel, T> factory);
}
