using Core.Application.Rpc.Procedures;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Rpc;

public class RpcClient : IRpcClient, IAsyncDisposable
{
	public event EventHandler<bool>? ServerConnectionChanged;
	public bool IsConnected => (this.Connection?.State == HubConnectionState.Connected;
	public bool IsConfigured { get; private set; }

    private readonly string endpoint;
    // Necessary because this.Connection instance is not intialized 
    // when procedures are reigstered in the child constructor
    private List<Action<HubConnection>> procedureRegistrations = new();
	private CancellationTokenSource reconnectTokenSource;

    public RpcClient(string endpoint)
    {
        this.endpoint = endpoint;
		this.reconnectTokenSource = new CancellationTokenSource();
    }
    
    protected HubConnection? Connection { get; private set; }

    public void Configure(string host)
    {
		var url = $"{host}/{endpoint}";
		this.Connection = new HubConnectionBuilder()
			.WithUrl(url)
			.Build();
		this.Connection.Closed += ex =>
		{
			this.BeginReconnecting(this.reconnectTokenSource.Token);
			return Task.CompletedTask;
		};
        foreach (var registerProcedure in procedureRegistrations)
		{
			registerProcedure(this.Connection);
		}
	}

    public virtual async Task Connect()
    {
		if (this.Connection == null)
        {
			var error = new Exception($"Cannot connect - client host is not configured. Use {nameof(IRpcClient.Configure)}");
			this.RaiseError(error, "Connection");
			return;
        }
        if (this.reconnectTokenSource.IsCancellationRequested)
        {
            this.reconnectTokenSource = new CancellationTokenSource();
        }
        try
		{
			await this.Connection.StartAsync();
			this.RaiseConnected();
		}
        catch (Exception ex)
		{
			this.RaiseError(ex, this.endpoint);
		}
    }

    public virtual async Task Disconnect()
    {
        if (this.Connection == null || !this.IsConnected)
        {
            return;
        }
		this.reconnectTokenSource.Cancel();
        await this.Connection.StopAsync();
		this.IsConnected = false;
    }

	public async ValueTask DisposeAsync()
    {
        if (this.Connection == null)
        {
            return;
        }
        await this.Connection.DisposeAsync();
		this.reconnectTokenSource.Dispose();
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
					this.RaiseError(exception, name);
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
					this.RaiseError(exception, name, a);
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
					this.RaiseError(exception, name, a, b);
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
					this.RaiseError(exception, name, a, b, c);
				}
			});
		});
	}

	protected async Task<RpcInvokeResult> InvokeAsync<T>(string name, T parameter)
	{
		try
		{
			await this.Connection.InvokeAsync(name, parameter);
			return RpcInvokeResult.Success;
		}
		catch (Exception exception)
		{
			this.RaiseError(exception, name, parameter);
			return RpcInvokeResult.Error;
		}
	}

	protected async Task<RpcInvokeResult<T>> InvokeAsync<T>(string name)
	{
		try
		{
			var result = await this.Connection.InvokeAsync<T>(name);
			return RpcInvokeResult<T>.Success(result);
		}
		catch (Exception exception)
		{
			this.RaiseError(exception, name);
			return RpcInvokeResult<T>.Error;
        }
	}

    public event EventHandler<RpcError>? Error;
    private void RaiseError(Exception exception, string procedure, params object[] arguments)
    {
        Console.WriteLine($"Error in '{procedure}': {exception.Message}");
        var error = new RpcError(exception, procedure, arguments);
        this.Error?.Invoke(this, error);
    }

    private void BeginReconnecting(CancellationToken cancellationToken)
    {
		this.RaiseDisconnected();
        var timer = new System.Timers.Timer(5000);
        timer.Elapsed += async (s, e) =>
        {
            if (cancellationToken.IsCancellationRequested)
            {
                timer.Stop();
                timer.Dispose();
            }
            try
            {
                await this.Connection!.StartAsync();
                if (this.Connection.State == HubConnectionState.Connected)
                {
					this.RaiseConnected();
                    timer.Stop();
                    timer.Dispose();
                }
            }
            catch (Exception) { }
        };
        timer.Start();
    }

	private void RaiseDisconnected()
	{
		ServerConnectionChanged?.Invoke(this, false);
	}

	private void RaiseConnected()
	{
		ServerConnectionChanged?.Invoke(this, true);
	}
}

public interface IRpcClient
{
	/// <summary>
	/// 'true' means connected; 'false' - disconnected;
	/// </summary>
	event EventHandler<bool>? ServerConnectionChanged;
	event EventHandler<RpcError>? Error;
	bool IsConnected { get; }
	bool IsConfigured { get; }
    void Configure(string host);
	Task Connect();
    Task Disconnect();
}
