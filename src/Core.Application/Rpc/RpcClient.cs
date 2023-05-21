using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Core.Application.Rpc;

public class RpcClient : IRpcClient, IAsyncDisposable
{
    public RpcClient(string url)
    {
        this.Connection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();
    }

    protected HubConnection Connection { get; }

    public virtual Task Start()
    {
        return this.Connection.StartAsync();
    }

    public virtual Task Stop()
    {
        return this.Connection.StopAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await this.Connection.DisposeAsync();
    }

    protected void AddProcedure<T>(string name, Func<T, Task> action)
    {
        this.Connection.On<T>(name, a =>
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
    }
    protected void AddProcedure<T1, T2>(string name, Func<T1, T2, Task> action)
    {
        this.Connection.On<T1, T2>(name, (a, b) =>
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
    }
    protected void AddProcedure<T1, T2, T3>(string name, Func<T1, T2, T3, Task> action)
    {
        this.Connection.On<T1, T2, T3>(name, (a, b, c) =>
        {
            try
            {
                action(a, b, c);
            }
            catch (Exception exception)
            {
                this.HandleError(exception, name, a, b);
            }
        });
    }

    public EventHandler<RpcError> Error;
    private void HandleError(Exception exception, string procedure, params object[] arguments)
    {
        Console.WriteLine($"Error in '{procedure}': {exception.Message}");
        var error = new RpcError(exception, procedure, arguments);
        this.Error?.Invoke(this, error);
    }
}

public interface IRpcClient
{
    Task Start();
    Task Stop();
}
