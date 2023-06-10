using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Enums;
using EMS.Witness.Services;
using static Core.Application.CoreApplicationConstants;

namespace EMS.Witness.Rpc;

public class StartlistClient : RpcClient, IStartlistClientProcedures, IStartlistClient
{
	private readonly WitnessState state;
	public event EventHandler<(StartlistEntry entry, CollectionAction action)>? Updated;
 
    public StartlistClient(WitnessState state) : base(RpcEndpoints.STARTLIST)
    {
        this.AddProcedure<StartlistEntry, CollectionAction>(nameof(this.Update), this.Update);
		this.state = state;
	}

    public Task Update(StartlistEntry entry, CollectionAction action)
	{
		this.Updated?.Invoke(this, (entry, action));
		return Task.CompletedTask;
	}

	public async Task<RpcInvokeResult<IEnumerable<StartlistEntry>>> Load()
	{
		return await this.InvokeAsync<IEnumerable<StartlistEntry>>(nameof(IStartlistHubProcedures.Get));
    }
}

public interface IStartlistClient : IRpcClient
{
    event EventHandler<(StartlistEntry entry, CollectionAction action)>? Updated;
    Task<RpcInvokeResult<IEnumerable<StartlistEntry>>> Load();
}