using Common.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Common.Blazor;

public class ExceptionHandler : ErrorBoundary
{
    [Inject]
    public IStringLocalizer StringLocalizer { get; set; } = default!;

    public string? LocalizedMessage
    {
        get
        {
            if (this.CurrentException == null)
            {
                return null;
            }
            if (this.CurrentException is DomainException domainException && domainException.Args.Any())
            {
                var localizedArguments = domainException.Args.Select(x => this.StringLocalizer[x]);
                return this.StringLocalizer[domainException.Message, domainException.Args];
            }
            return this.StringLocalizer[this.CurrentException.Message];
        }
    }
    public string? StackTrace
        => this.CurrentException?.StackTrace;

}
