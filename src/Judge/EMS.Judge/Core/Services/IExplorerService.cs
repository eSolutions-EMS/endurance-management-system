using Core.ConventionalServices;

namespace EMS.Judge.Core.Services;

public interface IExplorerService : ITransientService
{
    string SelectDirectory();

    string SelectFile();
}
