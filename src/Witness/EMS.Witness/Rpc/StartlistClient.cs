using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Enums;
using EMS.Witness.Services;
using Microsoft.AspNetCore.SignalR.Client;
using static Core.Application.CoreApplicationConstants;

namespace EMS.Witness.Rpc;

public class StartlistClient : RpcClient, IStartlistClientProcedures
{
	private readonly State state;
 
    public StartlistClient(State state) : base(RpcEndpoints.STARTLIST)
    {
        this.AddProcedure<StartlistEntry, CollectionAction>(nameof(this.Update), this.Update);
		this.state = state;
	}

    public Task Update(StartlistEntry entry, CollectionAction action)
	{
		var exising = this.state.Startlist.FirstOrDefault(x => x.Number == entry.Number); // TODO: use domain euqlas
		if (exising != null)
		{
			this.state.Startlist.Remove(exising);
		}
		if (action == CollectionAction.AddOrUpdate)
		{
			this.state.Startlist.Add(entry);
		}
		return Task.CompletedTask;
	}

	public override async Task FetchInitialState()
	{
		var startlist = await this.Connection.InvokeAsync<Startlist>(nameof(IStartlistHubProcedures.Get));
		this.state.Startlist.AddRange(startlist);
	}
}
