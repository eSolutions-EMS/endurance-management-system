using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EMS.Witness.Services;
using Microsoft.AspNetCore.SignalR.Client;

namespace EMS.Witness.Rpc;

public class StartlistClient : RpcClient, IStartlistClientProcedures
{
	private readonly State state;
 
    public StartlistClient(State state) : base("startlist-hub")
    {
        this.AddProcedure<StartModel>(nameof(this.AddEntry), this.AddEntry);
		this.state = state;
	}

    public void AddEntry(StartModel entry)
	{
		this.state.Startlist.Add(entry);
	}

	public override async Task FetchInitialState()
	{
		var startlist = await this.Connection.InvokeAsync<List<StartModel>>(nameof(IStartlistHubProcedures.Get));
		this.state.Startlist.AddRange(startlist);
	}
}
