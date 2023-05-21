using Core.Application.Rpc;
using Core.Application.Rpc.Procedures;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using System;
using System.Threading.Tasks;

namespace Test.Client
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var client = new StartlistClient("http://localhost:11337/startlist-hub");
			Task.Run(() => client.Start().GetAwaiter().GetResult());

			while (true)
			{
			}
		}
	}
}

public class StartlistClient : RpcClient, IStartlistProcedures
{
	public StartlistClient(string connectionUrl) : base(connectionUrl)
	{
		this.AddProcedure<StartModel>(nameof(this.AddEntry), this.AddEntry);
	}

	public Task AddEntry(StartModel entry)
	{
		Console.WriteLine($"#:{entry.Number}, Name: {entry.Name}, Date: {entry.Distance}");
		return Task.CompletedTask;
	}
}
