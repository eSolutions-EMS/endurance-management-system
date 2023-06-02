using Core.Application.Rpc;
using Core.Application.Services;
using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EMS.Witness.Shared.Toasts;
using System.Net.Http.Json;
using static Core.Application.CoreApplicationConstants;

namespace EMS.Witness.Services;

public class ApiService : IApiService
{
	private static string host = string.Empty;
    private readonly HttpClient httpClient;
	private readonly ToasterService toasterService;
	private readonly INetworkHandshakeService handshakeService;
	private readonly IEnumerable<IRpcClient> rpcClients;
	private readonly IPermissionsService permissionsService;

	public ApiService(
		IEnumerable<IRpcClient> rpcClients,
		IPermissionsService permissionsService,
		INetworkHandshakeService handshakeService,
		HttpClient httpClient,
		ToasterService toasterService)
    {
		this.rpcClients = rpcClients;
		this.permissionsService = permissionsService;
        this.httpClient = httpClient;
		this.toasterService = toasterService;
		this.handshakeService = handshakeService;
    }

	public async Task Handshake()
	{
		try
		{
			if (await this.permissionsService.HasNetworkPermissions())
			{
				var ip = await this.handshakeService.Handshake(Apps.WITNESS, new CancellationToken());
				if (ip == null)
				{
					var error = new Toast("Could not handshake", "Judge API address is null", UiColor.Danger, 10);
					this.toasterService.Add(error);
					return;
				}
				ConfigureApiHost(ip.ToString());
				var toast = new Toast(
					"Handshake successful",
					$"Connected to Judge API on '{ip}'",
					UiColor.Success,
					10);
				this.toasterService.Add(toast);
                foreach (var client in this.rpcClients)
                {
                    client.Configure(host);
                    await client.Start();
                }
            }
			else
			{
				var error = new Toast(
					"Network permission rejected",
					"eWitness app cannot operate without Network permissions. Grant permissions in device settings.",
					UiColor.Danger,
					10);
				this.toasterService.Add(error);
			}
		}
		catch (Exception exception)
		{
			this.ToastError(exception);
		}
	}

	public async Task<List<StartlistEntry>> GetStartlist()
    {
        try
        {
			return await this.httpClient.GetFromJsonAsync<List<StartlistEntry>>(this.BuildUrl(Api.STARTLIST))
				?? new List<StartlistEntry>();
		}
        catch (Exception exception)
        {
			this.ToastError(exception);
			return new List<StartlistEntry>();
        }
    }

	public async Task<bool> PostWitnessEvent(WitnessEvent witnessEvent)
    {
		try
		{
			var url = this.BuildUrl(Api.WITNESS);
            var result = await this.httpClient.PostAsJsonAsync(url, witnessEvent);
			if (!result.IsSuccessStatusCode)
			{
				throw new Exception($"Unsuccessful response: {result.StatusCode}");
			}
			return true;
		}
		catch (Exception exception)
		{
			this.ToastError(exception);
			return false;
		}
    }

	private void ToastError(Exception exception)
	{
		var toast = new Toast(exception.Message, exception.StackTrace, UiColor.Danger, 30);
		this.toasterService.Add(toast);
	}

	private string BuildUrl(string uri)
	{
		if (host == string.Empty)
		{
			var toast = new Toast(
				"Connection problem",
				"Cannot connect to Judge, handshake is not performed or was not successful",
				UiColor.Danger,
				10);
			this.toasterService.Add(toast);
		}
		return $"{host}/{uri}";
	}

	private void ConfigureApiHost(string ipAddress)
	{
		host = $"http://{ipAddress}:{NETWORK_API_PORT}";
	}
		

	public bool IsSuccessfulHandshake()
		=> host != string.Empty;

	public async Task FetchInitialState()
	{
		try
		{
			if (!this.IsSuccessfulHandshake())
			{
				await this.Handshake();
			}
			foreach (var client in this.rpcClients)
			{
				await client.FetchInitialState();
			}
		}
		catch (Exception exception)
		{
			this.ToastError(exception);
		}
		
	}
}

public interface IApiService : ISingletonService
{
	bool IsSuccessfulHandshake();
	Task Handshake();
	Task<List<StartlistEntry>> GetStartlist();
	Task<bool> PostWitnessEvent(WitnessEvent witnessEvent);
	Task FetchInitialState();
}
