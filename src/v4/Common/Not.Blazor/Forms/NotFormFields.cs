using Common.Domain;
using Common.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Not.Blazor.Forms;

public abstract class NotFormFields<T> : ComponentBase
    where T : DomainEntity
{
    /// <summary>
    /// Contains refs to the actual field components, necessary in order to render Mud validation messages from the DomainException
    /// This is a workaround until: https://github.com/eSolutions-EMS/endurance-management-system/issues/185
    /// </summary>
    protected Dictionary<string, MudValidationInjector> ValidationInjectors { get; set; } = new();

    public abstract T SubmitCreate();
    public abstract T SubmitUpdate();

    protected void RegisterInjector<TInput>(string field, Func<MudBaseInput<TInput>> mudInputInstanceGetter)
    {
        ValidationInjectors.Add(field, MudValidationInjector.Create(mudInputInstanceGetter));
    }

    public void AddValidationError(string field, string message)
    {
        if (!ValidationInjectors.TryGetValue(field, out var injector))
        {
            //TODO: use IDefaultNotifier or something
            throw ThrowHelper.Exception(
                $"Key '{field}' not found in {nameof(NotFormFields<T>)}.{nameof(ValidationInjectors)}. " +
                $"Make sure all field components have a ref pointer in there.");
        }

        injector.Inject(message);
    }
}

