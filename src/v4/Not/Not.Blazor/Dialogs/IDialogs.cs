using Not.Conventions;
using Not.Domain;
using Not.Blazor.TM.Forms.Components;

namespace Not.Blazor.Dialogs;

public interface IDialogs<T, TForm> : ITransientService
    where T : DomainEntity
    where TForm : NotForm<T>
{
    Task RenderCreate();
}
