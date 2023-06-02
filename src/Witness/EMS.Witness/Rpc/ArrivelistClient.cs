﻿using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;
using Core.Enums;
using EMS.Witness.Services;
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
		var result = await this.InvokeAsync<IEnumerable<ArrivelistEntry>>(nameof(IArrivelistHubProcedures.Get));
		if (result.IsSuccessful)
		{
            this.state.Arrivelist.Clear();
            this.state.Arrivelist.AddRange(result.Data!);
        }
	}

    public async Task<RpcInvokeResult> Save(IEnumerable<ArrivelistEntry> entries)
    {
		return await this.InvokeAsync(nameof(IArrivelistHubProcedures.Save), entries);
    }
}

public interface IArrivelistClient : IRpcClient
{
	Task<RpcInvokeResult> Save(IEnumerable<ArrivelistEntry> entries);
}
