using Common.Domain;
using Not.Blazor.Dialogs;
using Not.Blazor.Navigation;

namespace Not.Blazor.Forms;

public class NotFormNavigator<T, TFields> : IFormNavigator<T, TFields>
    where T : DomainEntity
    where TFields : NotFormFields<T>
{
    private readonly INotDialogService<T, TFields> _notDialogs;
    private readonly INavigator _navigator;

    public NotFormNavigator(INotDialogService<T, TFields> notDialogs, INavigator navigator)
    {
        _notDialogs = notDialogs;
        _navigator = navigator;
    }

    public async Task Create()
    {
        await _notDialogs.RenderCreate();
    }

    public Task Update(string endpoint, T entity)
    {
        _navigator.NavigateTo(endpoint, entity);
        return Task.CompletedTask;
    }
}
