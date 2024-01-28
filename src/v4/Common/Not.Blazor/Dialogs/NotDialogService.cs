using Common.Conventions;
using Common.Domain;
using Common.Services;
using Common.Utilities;
using MudBlazor;
using Not.Blazor.Forms;

namespace Not.Blazor.Dialogs;

public class NotDialogService : INotDialogService
{
    private readonly IDialogService _mudDialogService;
    private readonly ILocalizer _localizer;

    public NotDialogService(IDialogService mudDialogService, ILocalizer localizer)
    {
        _mudDialogService = mudDialogService;
        _localizer = localizer;
    }

    public async Task Create<T, TModel, TForm>(Func<TModel, T> factory)
        where T : DomainEntity
        where TModel : class, new()
        where TForm : FormContent
    {
        var parameters = new DialogParameters<CreateDialog<T, TModel, TForm>>
        {
            { x => x.Factory, factory }
        };

        var dialog = _mudDialogService.Show<CreateDialog<T, TModel, TForm>>(
            _localizer.Get("Create", " ", ReflectionHelper.GetName<T>()),
            parameters);
        await dialog.Result;
    }

    public async Task CreateChild<T, TModel, TForm>(Func<TModel, T> factory)
        where T : DomainEntity
        where TModel : class, new()
        where TForm : FormContent
    {
        var parameters = new DialogParameters<CreateChildDialog<T, TModel, TForm>>
        {   
            { x => x.Factory, factory }
        };

        var dialog = await _mudDialogService.ShowAsync<CreateChildDialog<T, TModel, TForm>>(
            _localizer.Get("Create", " ", ReflectionHelper.GetName<T>()),
            parameters);
        await dialog.Result;
    }
}

public interface INotDialogService : ITransientService
{
    Task Create<T, TModel, TForm>(Func<TModel, T> factory)
        where T : DomainEntity
        where TModel : class, new()
        where TForm : FormContent;
    Task CreateChild<T, TModel, TForm>(Func<TModel, T> factory)
        where T : DomainEntity
        where TModel : class, new()
        where TForm : FormContent;
}
