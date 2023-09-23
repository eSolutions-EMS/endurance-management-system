using Common.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Common.Blazor;

public class ExceptionHandler<T> : ErrorBoundary
{
    [Inject]
    public IStringLocalizer<T> StringLocalizer { get; set; } = default!;
    
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
                var localizedArgs = domainException
                    .Args
                    .Select(x => this.StringLocalizer[x])
                    .ToArray();
                return this.StringLocalizer[domainException.Message, localizedArgs];
            }
            return this.StringLocalizer[this.CurrentException.Message];
        }
    }
    public string? StackTrace
        => this.CurrentException?.StackTrace;

}
