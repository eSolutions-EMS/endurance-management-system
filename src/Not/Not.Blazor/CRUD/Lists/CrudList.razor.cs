using Not.Blazor.Components;
using Not.Blazor.CRUD.Forms;
using Not.Blazor.CRUD.Forms.Components;
using Not.Blazor.CRUD.Forms.Ports;
using Not.Blazor.CRUD.Lists.Ports;
using Not.Domain.Base;

namespace Not.Blazor.CRUD.Lists;

public partial class CrudList<T, TModel, TForm> : NComponent
    where T : AggregateRoot
    where TModel : IFormModel<T>, new()
    where TForm : NForm<TModel>
{
    [Inject]
    IListBehind<T> Behind { get; set; } = default!;

    [Inject]
    FormManager<TModel, TForm> FormNavigator { get; set; } = default!;

    [Parameter]
    public int? ParentId { get; set; }

    [Parameter]
    public string Name { get; set; } = default!;

    [Parameter, EditorRequired]
    public string UpdateRoute { get; set; } = default!;

    public string EmptyMessage { get; set; } = default!;

    protected override void OnInitialized()
    {
        GuardHelper.ThrowIfDefault(UpdateRoute);
        Name = Localizer.Get(Name ?? $"{typeof(T).Name}s");
        //TODO: RefactorLocalizer.Get to use string.Format
        EmptyMessage = Localizer.Get($"No {Name} have been created for this event");
    }

    protected override async Task OnInitializedAsync()
    {
        IEnumerable<object> args = ParentId != null ? [ParentId] : [];
        await Observe(Behind, args);
    }

    public async Task CreateHandler()
    {
        await FormNavigator.Create();
    }

    public async Task DeleteHandler(T item)
    {
        await Behind.Delete(item);
    }

    TModel CreateModel(T entity)
    {
        var model = new TModel();
        model.FromEntity(entity);
        return model;
    }
}
