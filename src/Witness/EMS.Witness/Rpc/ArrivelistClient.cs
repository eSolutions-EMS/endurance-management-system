using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;
using Core.Enums;
using EMS.Witness.Services;
using Microsoft.AspNetCore.SignalR.Client;
using static Core.Application.CoreApplicationConstants;

namespace EMS.Witness.Rpc;

public class ArrivelistClient : RpcClient, IArrivelistClient, IArrivelistClientProcedures
{
	private readonly IState state;

	public ArrivelistClient(IState state) : base(RpcEndpoints.ARRIVELIST)
    {
		this.state = state;
		this.AddProcedure<ArrivelistEntry, CollectionAction>(nameof(this.Update), this.Update);
	}

    public Task Update(ArrivelistEntry entry, CollectionAction action)
	{
		var exising = this.state.Arrivelist.FirstOrDefault(x => x.Number == entry.Number); // TODO: use domain euqlas
		if (exising != null)
		{
			this.state.Arrivelist.Remove(exising);
		}
		if (action == CollectionAction.AddOrUpdate)
		{
			this.state.Arrivelist.Add(entry);
		}
		return Task.CompletedTask;
	}

	public override async Task FetchInitialState()
	{
		var arrivelist = await this.Connection.InvokeAsync<IEnumerable<ArrivelistEntry>>(nameof(IArrivelistHubProcedures.Get));
		this.state.Arrivelist.Clear();
		this.state.Arrivelist.AddRange(arrivelist);
	}

    public async Task Save(IEnumerable<ArrivelistEntry> entries)
    {
		//TODO: Create base method and handle errors same as procedures
		await this.Connection.InvokeAsync(nameof(IArrivelistHubProcedures.Save), entries);
    }
}

public interface IArrivelistClient : IRpcClient
{
	Task Save(IEnumerable<ArrivelistEntry> entries);
}
