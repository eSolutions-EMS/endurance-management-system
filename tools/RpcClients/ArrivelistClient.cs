using Core.Application;
using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.Arrivelists;
using Core.Enums;

namespace EMS.Tools.RpcClients;

public class ArrivelistClient : RpcClient, IArrivelistClientProcedures
{
    public ArrivelistClient() : base(CoreApplicationConstants.RpcEndpoints.STARTLIST)
    {
        this.AddProcedure<ArrivelistEntry, CollectionAction>(nameof(this.Update), this.Update);
    }
    
    public Task Update(ArrivelistEntry entry, CollectionAction action)
    {
        Console.WriteLine($"Update - entry '{entry.Number}'");
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<ArrivelistEntry>> Get()
    {
        var result = await this.InvokeAsync<IEnumerable<ArrivelistEntry>>(nameof(IArrivelistHubProcedures.Get));
        if (!result.IsSuccessful)
        {
            Console.WriteLine("Get: Error");
            return null;
        }
        Console.WriteLine($"Get: {result.Data!.Count()}");
        return result.Data!;
    }

    public async Task Save(IEnumerable<ArrivelistEntry> entries)
    {
        var result = await this.InvokeAsync(nameof(IArrivelistHubProcedures.Save), entries);
        if (!result.IsSuccessful)
        {
            Console.WriteLine("Save: Error");
            return;
        }
        Console.WriteLine("Save: Success");
    }
}
