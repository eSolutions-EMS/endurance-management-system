using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.WitnessEvents;
using Core.Enums;
using EMS.Judge.Api.Configuration;
using EMS.Judge.Api.Rpc.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Judge.Api.Rpc.Services;

public class ArrivelistRpcService : IClientRpcService
{
    private readonly ManagerRoot managerRoot;
    private readonly IHubContext<ParticipantsHub, IParticipantsClientProcedures> hubContext;

    public ArrivelistRpcService(
        IJudgeServiceProvider provider,
        IHubContext<ParticipantsHub, IParticipantsClientProcedures> hubContext)
    {
        this.managerRoot = provider.GetRequiredService<ManagerRoot>();
        this.hubContext = hubContext;
        Witness.ParticipantChanged += (sender, args) => this.Update(args.number, args.action);
    }

    public void Update(string number, CollectionAction action)
    {
        var entry = this.managerRoot.GetParticipantEntry(number);
        if (entry == null)
        {
            return;
        }
        this.hubContext.Clients.All.Update(entry, action);
    }
}