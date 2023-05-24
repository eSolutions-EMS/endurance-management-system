using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager;
using Microsoft.AspNetCore.SignalR.Client;

namespace EMS.Witness.Rpc;
public class WitnessEventsClient : RpcClient, IWitnessEventClient
{
	public WitnessEventsClient() : base("witness-events")
	{
	}

	public async Task Add(WitnessEvent witnessEvent)
	{
		await this.Connection.InvokeAsync(nameof(IWitnessEventsHubProcedures.Add), witnessEvent);
	}
}

public interface IWitnessEventClient : IRpcClient
{
	Task Add(WitnessEvent witnessEvent);
}