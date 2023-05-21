using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EMS.Witness.Services;

namespace EMS.Witness.Rpc;
public class StartlistClient : RpcClient, IStartlistProcedures
{
	private readonly IState state;

	public StartlistClient(IState state) : base("startlist-hub")
    {
        this.AddProcedure<StartModel>(nameof(this.AddEntry), this.AddEntry);
		this.state = state;
	}

    public async Task AddEntry(StartModel entry)
	{
		this.state.Startlist.Add(entry);
	}
}
