using Core.Application.Rpc.Procedures;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Rpc;

public class RpcClient : IRpcClient, IAsyncDisposable
{
	private const int AUTOMATIC_RECONNECT_ATTEMPTS = 3;
    public event EventHandler<RpcConnectionStatus>? ServerConnectionChanged;
	public event EventHandler<string>? ServerConnectionInfo;
	public event EventHandler<RpcError>? Error;
	private System.Timers.Timer? _reconnectionTimer;
	private int _connectionClosedReconnectAttempts;

	public bool IsConnected => this.Connection?.State == HubConnectionState.Connected;

    private readonly RpcContext context;

    // Necessary because this.Connection instance is not intialized 
    // when procedures are reigstered in the child constructor
    private List<Action<HubConnection>> procedureRegistrations = new();
	private CancellationTokenSource? reconnectTokenSource;
	private readonly string _name;

    public RpcClient(RpcContext context)
    {
        this.context = context;
		_name = GetType().Name;
    }
    
    protected HubConnection? Connection { get; private set; }

    public virtual async Task Connect(string host)
    {
        await InternalConnect(host, 0);
	}

    public virtual async Task Disconnect()
    {
        if (this.Connection == null || !this.IsConnected)
        {
            return;
        }
		this.reconnectTokenSource!.Cancel();
		reconnectTokenSource!.Dispose();
        await this.Connection.StopAsync();
		RaiseDisconnected();
    }

	public async ValueTask DisposeAsync()
    {
        if (this.Connection == null)
        {
            return;
        }
		this.Connection.Reconnected -= HandleReconnected;
		this.Connection.Reconnecting -= HandleReconnecting;
		this.Connection.Closed -= HandleClosed;
		await this.Connection.DisposeAsync();
		this.reconnectTokenSource?.Dispose();
        // Might occasionally tigger ObjectDiscpossedException if timer.Elapsed attempts to run
        // during or after Dispose. See https://codereview.stackexchange.com/questions/223877/safe-dispose-of-timer
		// for potential solutions
        _reconnectionTimer?.Dispose();
    }

	private async Task InternalConnect(string host, int reconnectAttempts)
	{
		if (IsConnected)
		{
			ServerConnectionInfo?.Invoke(this, $"{this.GetType().Name} is already connected");
			return;
		}
		if (this.Connection == null)
		{
			this.ConfigureConnection(host);
		}
		try
		{
			this.reconnectTokenSource = new CancellationTokenSource();
			RaiseConnecting();
			await this.Connection!.StartAsync();
			this.RaiseConnected();
		}
		catch (Exception ex)
		{
			if (HasReachedReconnectionAttemptLimit(++reconnectAttempts))
			{
				RaiseDisconnected(ex);
				return;
			}
			await Task.Delay(TimeSpan.FromSeconds(5));
			await InternalConnect(host, reconnectAttempts);
		}
	}

	private void ConfigureConnection(string host)
	{
		context.Host = host;
		this.Connection = new HubConnectionBuilder()
			.WithUrl(this.context.Url)
			.Build();
		this.Connection.Reconnected += HandleReconnected;
		this.Connection.Reconnecting += HandleReconnecting;
		this.Connection.Closed += HandleClosed;
		foreach (var registerProcedure in procedureRegistrations)
		{
			registerProcedure(this.Connection);
		}
	}

	private Task HandleReconnected(string connectionId)
	{
		RaiseConnected($"SignalR automatic reconnected: {connectionId}");
		return Task.CompletedTask;
	}

	private Task HandleReconnecting(Exception exception)
	{
		RaiseReconnecting($"SignalR automatic reconnecting: {exception.Message}");
		return Task.CompletedTask;
	}

	private Task HandleClosed(Exception exception)
	{
		// This check is also necessary here, because if the server hub cannot be constructed (DI error for example)
		// SignalR keeps closing each connection to that hub as soon as it is created
		// Maybe test again with static connection?
		if (HasReachedReconnectionAttemptLimit(++_connectionClosedReconnectAttempts))
		{
			RaiseDisconnected(exception);
		}
		else
		{
			BeginReconnecting(this.reconnectTokenSource!.Token, exception, () => { _connectionClosedReconnectAttempts = 0; });

		}
		return Task.CompletedTask;
	}

    private void BeginReconnecting(CancellationToken cancellationToken, Exception? error, Action onSuccess)
    {
		this.RaiseDisconnected(error);
		RaiseConnecting();
		var reconnectAttempts = 0;
		_reconnectionTimer =  new System.Timers.Timer(TimeSpan.FromSeconds(10).TotalMilliseconds);
        _reconnectionTimer.Elapsed += async (s, e) =>
        {
            if (cancellationToken.IsCancellationRequested)
            {
				ServerConnectionInfo?.Invoke(this, "Reconecting stopped due to cancelation request");
                _reconnectionTimer.Stop();
                _reconnectionTimer.Dispose();
            }
            try
            {
                await this.Connection!.StartAsync();
                if (this.Connection.State == HubConnectionState.Connected)
                {
					this.RaiseConnected();
					onSuccess();
                    _reconnectionTimer.Stop();
                    _reconnectionTimer.Dispose();
                }
            }
            catch (Exception ex) 
			{
				RaiseReconnecting(ex);
			}
			finally
			{
				if (HasReachedReconnectionAttemptLimit(++reconnectAttempts))
				{
					RaiseDisconnected(new Exception("Automatic reconnection reached attempt limits. Try to reconnect manually"));
					_reconnectionTimer.Stop();
					_reconnectionTimer.Dispose();
				}
			}
        };
        _reconnectionTimer.Start();
    }

	private bool HasReachedReconnectionAttemptLimit(int attempts)
	{
		return attempts >= AUTOMATIC_RECONNECT_ATTEMPTS;
	}
	private void RaiseError(Exception exception, string? procedure, params object?[] arguments)
	{
		var message = procedure == null
			? $"RpcClient error : {exception.Message}"
			: $"RpcClient error in '{procedure}': {exception.Message}";
		Console.WriteLine(message);
		var error = new RpcError(exception, procedure, arguments);
		this.Error?.Invoke(this, error);
	}

	private void RaiseDisconnected(Exception? ex = default)
	{
		ServerConnectionChanged?.Invoke(_name, RpcConnectionStatus.Disconnected);
		ServerConnectionInfo?.Invoke(_name, ex?.Message ?? "Disconnected manually");
	}

	private void RaiseReconnecting(Exception ex)
	{
		ServerConnectionChanged?.Invoke(_name, RpcConnectionStatus.Connecting);
        ServerConnectionInfo?.Invoke(_name, $"{ex.Message}. Attempting to reconnect");
    }
	private void RaiseReconnecting(string message)
	{
		ServerConnectionChanged?.Invoke(_name, RpcConnectionStatus.Connecting);
        ServerConnectionInfo?.Invoke(_name, $"{message}. Attempting to reconnect");
    }
	private void RaiseConnecting()
	{
		ServerConnectionChanged?.Invoke(_name, RpcConnectionStatus.Connecting);
    }
	private void RaiseConnected(string? message = null)
	{
		ServerConnectionChanged?.Invoke(_name, RpcConnectionStatus.Connected);
		if (message != null)
		{
			ServerConnectionInfo?.Invoke(_name, message);
		}
    }

	protected void RegisterClientProcedure(string name, Func<Task> action)
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
	protected void RegisterClientProcedure<T>(string name, Func<T, Task> action)
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
	protected void RegisterClientProcedure<T1, T2>(string name, Func<T1, T2, Task> action)
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
	protected void RegisterClientProcedure<T1, T2, T3>(string name, Func<T1, T2, T3, Task> action)
	{
		this.procedureRegistrations.Add(connection =>
		{
			connection.On<T1, T2, T3>(name, (a, b, c) =>
			{
				try
				{
					action(a, b, c);
				}
				catch (Exception exception)
				{
					this.RaiseError(exception, name, a, b, c);
				}
			});
		});
	}

	protected async Task<RpcInvokeResult> InvokeHubProcedure<T>(string name, T parameter)
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

	protected async Task<RpcInvokeResult> InvokeHubProcedure<T1, T2>(string name, T1 parameter1, T2 parameter2)
	{
		try
		{
			await this.Connection.InvokeAsync(name, parameter1, parameter2);
			return RpcInvokeResult.Success;
		}
		catch (Exception exception)
		{
			this.RaiseError(exception, name, parameter1, parameter2);
			return RpcInvokeResult.Error;
		}
	}

	protected async Task<RpcInvokeResult<T>> InvokeHubProcedure<T>(string name)
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
}

public interface IRpcClient
{
	/// <summary>
	/// 'true' means connected; 'false' - disconnected;
	/// </summary>
	event EventHandler<RpcConnectionStatus>? ServerConnectionChanged;
	event EventHandler<string>? ServerConnectionInfo;
	event EventHandler<RpcError>? Error;
	bool IsConnected { get; }
	Task Connect(string host);
    Task Disconnect();
}

public enum RpcConnectionStatus
{
	Disconnected = 0,
	Connecting = 1,
	Connected = 2,
}
