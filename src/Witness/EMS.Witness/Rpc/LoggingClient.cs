using Core.Application.Http;
using EMS.Witness.Services;
using System.Net.Http.Json;

namespace EMS.Witness.Rpc;

public class LoggingClient : IWitnessLogger
{
	private IWitnessState _state;
    private readonly IHttpClientFactory _httpClientFactory;

    public LoggingClient(IWitnessState state, IHttpClientFactory httpClientFactory)
	{
		_state = state;
        _httpClientFactory = httpClientFactory;
    }

	public async Task Log(string functionality, Exception exception)
	{
		if (_state.HostIp == null)
		{
			return;
		}
		var clientId = GetClientId();
		var client = _httpClientFactory.CreateClient();
		var request = ClientLogRequest.Create($"{clientId}:{functionality}", exception);
		var url = $"http://{_state.HostIp}:11337/client-logging"; 
        await client.PostAsJsonAsync(url, request);
    }

    private string GetClientId() => $"{DeviceInfo.Current.Manufacturer}-{DeviceInfo.Current.Name}-{DeviceInfo.Current.Version}";
}

public interface IWitnessLogger
{
	Task Log(string functionality, Exception exception);
}