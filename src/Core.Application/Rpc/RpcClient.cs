using Core.ConventionalServices;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Rpc;

public class RpcClient : IRpcClient, IAsyncDisposable
{
    private readonly string endpoint;
    // Necessary because this.Connection instance is not intialized 
    // when procedures are reigstered in the child constructor
    private List<Action<HubConnection>> procedureRegistrations = new();

    public RpcClient(string endpoint)
    {
        this.endpoint = endpoint;
    }
    
    protected HubConnection? Connection { get; private set; }

    public void Configure(string host)
    {
        if (this.Connection != null)
        {
            return;
		}
		var url = $"{host}/{endpoint}";
		this.Connection = new HubConnectionBuilder()
			.WithUrl(url)
			.Build();
		foreach (var registerProcedure in procedureRegistrations)
		{
			registerProcedure(this.Connection);
		}
	}

    public virtual async Task Start()
    {
        if (this.Connection == null)
        {
            return;
        }
		try
		{
			await this.Connection.StartAsync();
		}
        catch (Exception ex)
		{
			this.HandleError(ex, this.endpoint);
		}
    }

    public virtual async Task Stop()
    {
        if (this.Connection == null)
        {
            return;
        }
        await this.Connection.StopAsync();
    }

	public virtual Task FetchInitialState()
	{
        return Task.CompletedTask;
	}

	public async ValueTask DisposeAsync()
    {
        if (this.Connection == null)
        {
            return;
        }
        await this.Connection.DisposeAsync();
    }

	protected void AddProcedure(string name, Func<Task> action)
	{
		this.procedureRegistrations.Add(connection =>
		{
			connection.On(name, () =>
			{
				try
				{
					action();
				}
				catch (Exception exception)
				{
					this.HandleError(exception, name);
				}
			});
		});
	}
	protected void AddProcedure<T>(string name, Func<T, Task> action)
	{
		this.procedureRegistrations.Add(connection =>
		{
			connection.On<T>(name, a =>
			{
				try
				{
					action(a);
				}
				catch (Exception exception)
				{
					this.HandleError(exception, name, a);
				}
			});
		});
	}
    protected void AddProcedure<T1, T2>(string name, Func<T1, T2, Task> action)
    {
		this.procedureRegistrations.Add(connection =>
		{
			connection.On<T1, T2>(name, (a, b) =>
			{
				try
				{
					action(a, b);
				}
				catch (Exception exception)
				{
					this.HandleError(exception, name, a, b);
				}
			});
		});
	}
    protected void AddProcedure<T1, T2, T3>(string name, Func<T1, T2, T3, Task> action)
    {
		this.procedureRegistrations.Add(connection =>
		{
			connection.On<T1, T2, T3>(name, (a, b, c) =>
			{
				try
				{
					action(a, b,c);
				}
				catch (Exception exception)
				{
					this.HandleError(exception, name, a, b, c);
				}
			});
		});
	}

    public event EventHandler<RpcError>? Error;
    private void HandleError(Exception exception, string procedure, params object[] arguments)
    {
        Console.WriteLine($"Error in '{procedure}': {exception.Message}");
        var error = new RpcError(exception, procedure, arguments);
        this.Error?.Invoke(this, error);
    }
}

public interface IRpcClient
{
	event EventHandler<RpcError>? Error;
    void Configure(string host);
	Task Start();
    Task Stop();
    Task FetchInitialState();
}
