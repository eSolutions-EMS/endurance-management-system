using Core.Application.Api;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace EMS.Judge.Api.Hubs
{
	public class StartlistHub : Hub<IStartlistClient>
	{
		public async Task SendEntry(StartModel entry)
		{
			await Clients.All.AddEntry(entry);
		}

        public override async Task OnConnectedAsync()
        {
			Console.WriteLine($"Connected: {this.Context.ConnectionId}");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Disconnected: {this.Context.ConnectionId}");
        }
    }
}
