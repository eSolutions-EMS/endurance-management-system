using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Application.Services;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Participants;
using Core.Enums;
using static Core.Application.CoreApplicationConstants;

namespace EMS.Witness.Rpc;

public class ParticipantsClient: RpcClient, IParticipantsClient, IParticipantsClientProcedures
{
	public event EventHandler<(ParticipantEntry entry, CollectionAction action)>? Updated;
	public event EventHandler<IEnumerable<ParticipantEntry>>? Loaded;

	public ParticipantsClient(IHandshakeService handshakeService)
		: base(
			new RpcContext(Apps.WITNESS, RpcProtocls.Http, NetworkPorts.JUDGE_SERVER, RpcEndpoints.PARTICIPANTS),
			handshakeService)
    {
		this.AddProcedure<ParticipantEntry, CollectionAction>(nameof(this.Update), this.Update);
	}

    public Task Update(ParticipantEntry entry, CollectionAction action)
    {
        this.Updated?.Invoke(this, (entry, action));
        return Task.CompletedTask;
    }

    public async Task<RpcInvokeResult<IEnumerable<ParticipantEntry>>> Load()
	{
		return await this.InvokeAsync<IEnumerable<ParticipantEntry>>(nameof(IParticipantstHubProcedures.Get));
	}

    public async Task<RpcInvokeResult> Send(IEnumerable<ParticipantEntry> entries, WitnessEventType type)
    {
		return await this.InvokeAsync(nameof(IParticipantstHubProcedures.Witness), entries, type);
    }
}

public interface IParticipantsClient : IRpcClient
{
	event EventHandler<(ParticipantEntry entry, CollectionAction action)>? Updated;
	Task<RpcInvokeResult<IEnumerable<ParticipantEntry>>> Load();
	Task<RpcInvokeResult> Send(IEnumerable<ParticipantEntry> entries, WitnessEventType type);
}
