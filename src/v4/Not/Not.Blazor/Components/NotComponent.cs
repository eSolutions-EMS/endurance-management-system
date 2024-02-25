using Common.Services;
using Microsoft.AspNetCore.Components;

namespace Not.Blazor.Components;

public class NotComponent : ComponentBase
{
    [Inject]
    protected ILocalizer Localizer { get; set; } = default!;
}
