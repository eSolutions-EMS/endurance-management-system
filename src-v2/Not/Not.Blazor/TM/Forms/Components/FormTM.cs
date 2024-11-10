using MudBlazor;
using Not.Blazor.Components;
using Not.Notifier;

namespace Not.Blazor.TM.Forms.Components;

public abstract class FormTM<T> : NotComponent
{
    public abstract void RegisterValidationInjectors();

    /// <summary>
    /// Contains refs to the actual field components, necessary in order to render Mud validation messages from the DomainException
    /// This is a workaround until: https://github.com/eSolutions-EMS/endurance-management-system/issues/185
    /// </summary>
    protected Dictionary<string, List<MudValidationInjector>> ValidationInjectors { get; set; } =
        [];
    [Parameter]
    public T Model { get; set; } = default!;

    protected override void OnInitialized()
    {
        RegisterValidationInjectors();
    }

    protected void RegisterInjector<TInput>(
        string field,
        Func<MudBaseInput<TInput>> mudInputInstanceGetter
    )
    {
        var injector = MudValidationInjector.Create(mudInputInstanceGetter);
        AddInjector(field, injector);
    }

    protected void RegisterInjector<TInput>(
        string field,
        Func<IMudBaseInputWrapper<TInput>> mudInputWrapper
    )
    {
        var injector = MudValidationInjector.Create(mudInputWrapper);
        AddInjector(field, injector);
    }

    protected void RegisterInjector<TInput>(
        string field,
        Func<MudPicker<TInput>> mudInputInstanceGetter
    )
    {
        var injector = MudValidationInjector.Create(mudInputInstanceGetter);
        AddInjector(field, injector);
    }

    protected void RegisterInjector(string field, Func<NotSwitch> mudInputInstanceGetter)
    {
        var injector = MudValidationInjector.Create<bool, bool>(mudInputInstanceGetter);
        AddInjector(field, injector);
    }

    public async Task AddValidationError(string? field, string message)
    {
        if (field == null)
        {
            NotifyHelper.Warn(message);
            return;
        }
        if (!ValidationInjectors.TryGetValue(field, out var injectors))
        {
            throw GuardHelper.Exception(
                $"Key '{field}' not found in {nameof(FormTM<T>)}.{nameof(ValidationInjectors)}. "
                    + $"Make sure all field components have a ref pointer in there."
            );
        }

        foreach (var injector in injectors)
        {
            injector.Inject(message);
        }
        await Render();
    }

    void AddInjector(string field, MudValidationInjector injector)
    {
        if (!ValidationInjectors.TryAdd(field, [injector]))
        {
            ValidationInjectors[field].Add(injector);
        }
    }
}
