using Common.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Not.Blazor.Forms;

public abstract class NotFormFields<T> : ComponentBase, INotFormFields
    where T : class
{
    [Parameter]
    public T Model { get; set; } = default!;

    [Parameter]
    public object ObjectModel
    { 
        get => Model;
        set
        {
            if (value is not T model)
            {
                throw ThrowHelper.Exception(
                    $"Invalid model value '{value.GetType()}'. {this.GetType()} expects a model of type '{typeof(T)}'");
            }
            Model = model;
        }
    }

    /// <summary>
    /// Contains refs to the actual field components, necessary in order to render Mud validation messages from the DomainException
    /// This is a workaround until: https://github.com/eSolutions-EMS/endurance-management-system/issues/185
    /// </summary>
    protected Dictionary<string, MudValidationInjector> ValidationInjectors { get; set; } = new();

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

public interface INotFormFields
{
    object ObjectModel { get; set; }
}

