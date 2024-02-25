using Common.Domain;
using Common.Helpers;
using MudBlazor;
using Not.Blazor.Components;
using Not.Blazor.Forms;

namespace Not.Blazor.TM.Forms.Components;

public abstract class NotForm<T> : NotComponent, ICreateForm<T>, IUpdateForm<T>
    where T : DomainEntity
{
    /// <summary>
    /// Contains refs to the actual field components, necessary in order to render Mud validation messages from the DomainException
    /// This is a workaround until: https://github.com/eSolutions-EMS/endurance-management-system/issues/185
    /// </summary>
    protected Dictionary<string, MudValidationInjector> ValidationInjectors { get; set; } = new();

    public abstract T SubmitCreate();
    public abstract T SubmitUpdate();
    public abstract void SetUpdateModel(T entity);
    public abstract void RegisterValidationInjectors();

    protected override void OnInitialized()
    {
        RegisterValidationInjectors();
    }

    protected void RegisterInjector<TInput>(string field, Func<MudBaseInput<TInput>> mudInputInstanceGetter)
    {
        ValidationInjectors.Add(field, MudValidationInjector.Create(mudInputInstanceGetter));
    }

    public async Task AddValidationError(string? field, string message)
    {
        if (field == null)
        {
            // TODO: fix by adding toaster dependency to fallback to when Property is null
            // i.e a general domain exception is raised, rather than one tied to a specific form field
            throw new NotImplementedException($"Add INotifier fallback!: {message}");
        }

        if (!ValidationInjectors.TryGetValue(field, out var injector))
        {
            //TODO: use IDefaultNotifier or something
            throw ThrowHelper.Exception(
                $"Key '{field}' not found in {nameof(NotForm<T>)}.{nameof(ValidationInjectors)}. " +
                $"Make sure all field components have a ref pointer in there.");
        }

        injector.Inject(message);
        await InvokeAsync(StateHasChanged);
    }

    internal void TriggerRender()
    {
        StateHasChanged();
    }
}
