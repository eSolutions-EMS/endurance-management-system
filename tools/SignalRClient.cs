using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using Microsoft.AspNetCore.SignalR.Client;
using System.Runtime.CompilerServices;

namespace ems_tools
{
    public class SignalRClient : RpcClient, IStartlistProcedures
    {
        public Task Result = new Task(() => { });
        private Action action = () => {};

        public SignalRClient() : base("startlist-hub")
        {
            // this.AddProcedure<IEnumerable<StartModel>>(nameof(this.Receive), this.Receive);
        }

        public Task AddEntry(StartModel entry)
        {
            return Task.CompletedTask;
        }

        public IEnumerable<StartModel> Receive(IEnumerable<StartModel> startlist)
        {
            this.action();
            return startlist;
        }

        public async Task FetchStartlist()
        {
            var a = await this.Connection.InvokeAsync<IEnumerable<StartModel>>(nameof(this.Receive));
            await new Task(this.action);
        }
    }
}
