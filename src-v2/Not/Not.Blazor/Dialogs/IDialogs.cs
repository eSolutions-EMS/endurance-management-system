using Not.Blazor.TM.Forms.Components;
using Not.Injection;

namespace Not.Blazor.Dialogs;

public interface IDialogs<T, TForm> : ITransient
    where TForm : FormTM<T>
{
    Task RenderCreate();
}
