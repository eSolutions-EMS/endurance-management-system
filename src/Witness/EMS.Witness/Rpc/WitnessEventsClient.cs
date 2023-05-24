using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager;
using Microsoft.AspNetCore.SignalR.Client;
using static Core.Application.CoreApplicationConstants;

namespace EMS.Witness.Rpc;
public class WitnessEventsClient : RpcClient, IWitnessEventClient
{
	public WitnessEventsClient() : base(RpcEndpoints.WITNESS_EVENTS)
	{
	}

	public async Task Add(WitnessEvent witnessEvent)
	{
		await this.Connection.SendAsync(nameof(IWitnessEventsHubProcedures.Add), witnessEvent);
	}
}

public interface IWitnessEventClient : IRpcClient
{
	Task Add(WitnessEvent witnessEvent);
}
