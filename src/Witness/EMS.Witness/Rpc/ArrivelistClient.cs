using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;
using Core.Enums;
using static Core.Application.CoreApplicationConstants;

namespace EMS.Witness.Rpc;

public class ArrivelistClient : RpcClient, IArrivelistClient
{
	public event EventHandler<(ArrivelistEntry entry, CollectionAction action)>? Updated;
	public event EventHandler<IEnumerable<ArrivelistEntry>>? Loaded;

	public ArrivelistClient() : base(RpcEndpoints.ARRIVELIST)
    {
	}

	public async Task<RpcInvokeResult<IEnumerable<ArrivelistEntry>>> Load()
	{
		return await this.InvokeAsync<IEnumerable<ArrivelistEntry>>(nameof(IArrivelistHubProcedures.Get));
	}

    public async Task<RpcInvokeResult> Save(IEnumerable<ArrivelistEntry> entries)
    {
		return await this.InvokeAsync(nameof(IArrivelistHubProcedures.Save), entries);
    }
}

public interface IArrivelistClient : IRpcClient
{
	event EventHandler<(ArrivelistEntry entry, CollectionAction action)>? Updated;
	Task<RpcInvokeResult<IEnumerable<ArrivelistEntry>>> Load();
	Task<RpcInvokeResult> Save(IEnumerable<ArrivelistEntry> entries);
}
