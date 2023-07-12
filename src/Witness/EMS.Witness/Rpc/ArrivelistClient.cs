using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.ParticipantEntries;
using Core.Enums;
using static Core.Application.CoreApplicationConstants;

namespace EMS.Witness.Rpc;

public class ParticipantsClient: RpcClient, IParticipantsClient
{
	public event EventHandler<(ParticipantEntry entry, CollectionAction action)>? Updated;
	public event EventHandler<IEnumerable<ParticipantEntry>>? Loaded;

	public ParticipantsClient() : base(RpcEndpoints.PARTICIPANTS)
    {
	}

	public async Task<RpcInvokeResult<IEnumerable<ParticipantEntry>>> Load()
	{
		return await this.InvokeAsync<IEnumerable<ParticipantEntry>>(nameof(IParticipantstHubProcedures.Get));
	}

    public async Task<RpcInvokeResult> Save(IEnumerable<ParticipantEntry> entries)
    {
		return await this.InvokeAsync(nameof(IParticipantstHubProcedures.Save), entries);
    }
}

public interface IParticipantsClient : IRpcClient
{
	event EventHandler<(ParticipantEntry entry, CollectionAction action)>? Updated;
	Task<RpcInvokeResult<IEnumerable<ParticipantEntry>>> Load();
	Task<RpcInvokeResult> Save(IEnumerable<ParticipantEntry> entries);
}
