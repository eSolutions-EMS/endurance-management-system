using Common.Domain;
using Common.Helpers;
using Microsoft.AspNetCore.Components;

namespace Not.Blazor.Forms;

public abstract class UpdatePage<T> : ComponentBase
    where T : DomainEntity
{
    protected NotFormFields<T>? Fields { get; set; }

    protected abstract Task Update();

    protected async Task TryUpdate()
    {
        ThrowHelper.ThrowIfNull(Fields);

        try
        {
            await Update();
        }
        catch (DomainException domainException)
        {
            await Fields.AddValidationError(domainException.Property, domainException.Message);
        }
    }
}
