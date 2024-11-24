using Not.Safe;
using NTS.Judge.ACL.Adapters;
using NTS.Judge.Blazor.Shared.Components.SidePanels;
using NTS.Judge.Core.Start;

namespace NTS.Judge.Core.Behinds.Adapters;

public class CoreBehind : ICoreBehind
{
    readonly IEmsImporter _emsImporter;
    readonly ICoreStarter _coreStarter;

    public CoreBehind(IEmsImporter emsImporter, ICoreStarter coreStarter)
    {
        _emsImporter = emsImporter;
        _coreStarter = coreStarter;
    }

    public bool IsStarted { get; private set; }

    public Task<bool> Start()
    {
        return SafeHelper.Run(SafeStart);
    }

    public Task Import(string contents)
    {
        return SafeHelper.Run(() => _emsImporter.ImportCore(contents));
    }

    async Task<bool> SafeStart()
    {
        await _coreStarter.Start();
        return IsStarted = true;
    }
}
