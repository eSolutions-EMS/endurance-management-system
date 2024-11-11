using Not.Injection;

namespace Not.Blazor.Print;

public interface IPrintInterop : ITransient
{
    Task OpenPrintDialog();
}
