using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.WitnessEvents;
using Core.Enums;
using EMS.Judge.Api.Configuration;
using EMS.Judge.Api.Rpc.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EMS.Judge.Api.Rpc.Services;

public class StartlistRpcService : IClientRpcService, IDisposable
{
    private readonly ManagerRoot _managerRoot;
    private readonly IHubContext<StartlistHub, IClientProcedures> _hub;
    
    public StartlistRpcService(
        IHubContext<StartlistHub, IClientProcedures> hub,
        IJudgeServiceProvider judgeServiceProvider)
    {
        _hub = hub;
        _managerRoot = judgeServiceProvider.GetRequiredService<ManagerRoot>();
        Witness.StartlistChanged += SendStartlistEntryUpdate;
        Witness.ParticipantChanged += SendParticipantEntryUpdate;
    }

    public void SendStartlistEntryUpdate(object? _, (string Number, CollectionAction Action) args)
    {
        var entry = this._managerRoot.GetStarlistEntry(args.Number);
        if (entry == null)
        {
            return;
        }
        _hub.Clients.All.ReceiveEntry(entry, args.Action);
    }

    public void SendParticipantEntryUpdate(object? _, (string Number, CollectionAction Action) args)
    {
        var entry = this._managerRoot.GetParticipantEntry(args.Number);
        if (entry == null)
        {
            return;
        }
        _hub.Clients.All.ReceiveEntryUpdate(entry, args.Action);
    }

    public void Dispose()
    {
        Witness.StartlistChanged -= SendStartlistEntryUpdate;
        Witness.ParticipantChanged -= SendParticipantEntryUpdate;
    }
}
