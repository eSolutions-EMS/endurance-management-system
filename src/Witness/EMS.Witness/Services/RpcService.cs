using Core.Application.Rpc;
using Core.Application.Services;
using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EMS.Witness.Platforms.Services;
using EMS.Witness.Shared.Toasts;
using System.Net.Http.Json;
using static Core.Application.CoreApplicationConstants;

namespace EMS.Witness.Services;

public class RpcService: IRpcService
{
	private const string ALEX_HOME_WORKSTATION_IP = "192.168.0.36";
	private static string host = string.Empty;
    private readonly HttpClient httpClient;
	private readonly IToaster toaster;
	private readonly IHandshakeService handshakeService;
	private readonly IEnumerable<IRpcClient> rpcClients;
	private readonly IPermissionsService permissionsService;
	private readonly WitnessContext context;
	private bool isHandshaking;

	public RpcService(
		IWitnessContext context,
		IEnumerable<IRpcClient> rpcClients,
		IPermissionsService permissionsService,
		IHandshakeService handshakeService,
		HttpClient httpClient,
		IToaster toaster)
    {
		this.context = (WitnessContext)context;
		this.rpcClients = rpcClients;
		this.permissionsService = permissionsService;
        this.httpClient = httpClient;
		this.toaster = toaster;
		this.handshakeService = handshakeService;
    }

	public async Task Handshake()
	{
		if (this.isHandshaking)
		{
			return;
		}
		if (!await this.permissionsService.HasNetworkPermissions())
		{
            this.toaster.Add(
                "Network permission rejected",
                "eWitness app cannot operate without Network permissions. Grant permissions in device settings.",
                UiColor.Danger);
			return;
        }

		this.isHandshaking = true;
		try
		{
			await this.ConfigureServerHost();
			await this.ConnectRpcClients();
		}
		catch (Exception exception)
		{
			this.ToastError(exception);
		}
		finally
		{
			this.context.RaiseIsHandshakingEvent(false);
		}
	}

	private async Task ConfigureServerHost()
	{
#if DEBUG
        ConfigureApiHost(ALEX_HOME_WORKSTATION_IP);
#else
		if (host == string.Empty)
        {
            var ip = await this.handshakeService.Handshake(Apps.WITNESS, new CancellationToken());
            if (ip == null)
            {
                this.toaster.Add("Could not handshake", "Judge Server broadcast does not contain IP address", UiColor.Danger);
                return;
            }
            ConfigureApiHost(ip.ToString());
        }
#endif
    }

	private async Task ConnectRpcClients()
	{
        foreach (var client in this.rpcClients.Where(x => !x.IsConnected))
        {
            if (!client.IsConfigured)
            {
                client.Configure(host);
            }
            await client.Connect();
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
		this.toaster.Add(exception.Message, exception?.StackTrace, UiColor.Danger, 30);
	}

	private string BuildUrl(string uri)
	{
		if (host == string.Empty)
		{
			this.toaster.Add(
				"Connection problem",
                "Cannot connect to Judge, handshake is not performed or was not successful",
                UiColor.Danger);
		}
		return $"{host}/{uri}";
	}

	private void ConfigureApiHost(string ipAddress)
	{
		host = $"http://{ipAddress}:{NETWORK_API_PORT}";
	}
		

	public bool IsSuccessfulHandshake()
		=> host != string.Empty;
}

public interface IRpcService : ISingletonService
{
	bool IsSuccessfulHandshake();
	Task Handshake();
	Task<List<StartlistEntry>> GetStartlist();
	Task<bool> PostWitnessEvent(WitnessEvent witnessEvent);
}
