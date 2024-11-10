using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Not.Blazor.Components;

public class ExceptionHandler<T> : ErrorBoundary
{
    [Inject]
    public IStringLocalizer<T> StringLocalizer { get; set; } = default!;
    public string? Message => CurrentException?.Message;
    public string? StackTrace => CurrentException?.StackTrace;
}
