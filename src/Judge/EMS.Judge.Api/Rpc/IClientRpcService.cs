using Core.ConventionalServices;

namespace EMS.Judge.Api.Rpc;

/// <summary>
/// Event listeners that can invoke Client RPCs. Called once on app startup
/// to allow them to register event handlers.
/// </summary>
public interface IClientRpcService : ISingletonService
{
}
