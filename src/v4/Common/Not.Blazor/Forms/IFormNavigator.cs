using Common.Conventions;
using Common.Domain;

namespace Not.Blazor.Forms;

public interface IFormNavigator<T, TFields> : ITransientService
    where T : DomainEntity
    where TFields : NotFormFields<T>
{
    Task Create();
    Task Update(string endpoint, T entity);
}
