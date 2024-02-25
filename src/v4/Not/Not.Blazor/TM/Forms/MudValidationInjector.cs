using Not.Utilities;
using MudBlazor;
using System.Reflection;

namespace Not.Blazor.TM.Forms;

// Breaks Not* convention, because this is a workaround until 
// event-driven validation is implemented for MudBaseInput<T>
public class MudValidationInjector
{
    private static readonly Type LIST_OF_STRING_TYPE = typeof(List<string>);

    protected Func<object> InstanceGetter { get; }
    protected PropertyInfo ErrorProperty { get; }
    protected PropertyInfo ErrorTextProperty { get; }
    protected PropertyInfo ValidationErrorsProperty { get; }
    protected MethodInfo AddValidationErrorMethod { get; }

    private MudValidationInjector(Type mudInputType, Func<object> mudInputInstanceGetter)
    {
        InstanceGetter = mudInputInstanceGetter;
        // Generic parameter doesn't matter here as it's only used to obtain property names with nameof()
        ErrorProperty = mudInputType.Property(nameof(MudBaseInput<string>.Error));
        ErrorTextProperty = mudInputType.Property(nameof(MudBaseInput<string>.ErrorText));
        ValidationErrorsProperty = mudInputType.Property(nameof(MudBaseInput<string>.ValidationErrors));
        AddValidationErrorMethod = LIST_OF_STRING_TYPE.Method(nameof(List<string>.Add));
    }

    public void Inject(string message)
    {
        var mudBaseInput = InstanceGetter();

        ErrorProperty.Set(mudBaseInput, true);
        ErrorTextProperty.Set(mudBaseInput, message);
        var validationErrorsList = ValidationErrorsProperty.Get(mudBaseInput)
            ?? throw new Exception($"MudBlazor input component's ValidationErrors property is null'");
        AddValidationErrorMethod.Invoke(validationErrorsList, new object[] { message });
    }

    public static MudValidationInjector Create<T>(Func<MudBaseInput<T>> getter)
    {
        return new MudValidationInjector(typeof(MudBaseInput<T>), getter);
    }
}
