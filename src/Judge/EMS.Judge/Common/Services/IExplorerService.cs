using Core.ConventionalServices;

namespace EMS.Judge.Common.Services;

public interface IExplorerService : ITransientService
{
    string SelectDirectory();

    string SelectFile();
}
