using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Core.Enums;
using Core.Models;
using EMS.Witness.Rpc;

namespace EMS.Witness.Services;
public class StartlistService : IStartlistService
{
    private readonly IWitnessContext context;
    private readonly IStartlistClient startlistClient;

    public StartlistService(IWitnessContext context, IStartlistClient startlistClient)
    {
        this.context = context;
        this.startlistClient = startlistClient;
    }

    public ObservableCollection<StartlistEntry> Startlist => this.context.Startlist;

    public async Task Load()
    {
        var result = await this.startlistClient.Load();
        if (result.IsSuccessful)
        {
            this.Startlist.Clear();
            this.Startlist.AddRange(result.Data!);
        }
    }

    public void Update(StartlistEntry entry, CollectionAction action)
    {
        this.Startlist.Update(entry, action);
    }
}

public interface IStartlistService : ISingletonService
{
    ObservableCollection<StartlistEntry> Startlist { get; }
    Task Load();
    void Update(StartlistEntry entry, CollectionAction action);
}
