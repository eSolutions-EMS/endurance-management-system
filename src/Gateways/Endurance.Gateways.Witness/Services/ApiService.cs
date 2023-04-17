using Endurance.Gateways.Witness.Models;
using Endurance.Gateways.Witness.Shared.Toasts;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using System.Net.Http.Json;
using System.Text.Json;
using static EnduranceJudge.Application.ApplicationConstants;

namespace Endurance.Gateways.Witness.Services;

public class ApiService : IApiService
{
    private readonly HttpClient httpClient;
	private readonly ToasterService toasterService;

	public ApiService(HttpClient httpClient, ToasterService toasterService)
    {
        this.httpClient = httpClient;
		this.toasterService = toasterService;
	}

    public async Task<List<StartModel>> GetStartlist()
    {
        try
        {
			return await this.httpClient.GetFromJsonAsync<List<StartModel>>(Api.STARTLIST);
		}
        catch (Exception exception)
        {
			this.ToastError(exception);
			return null;
        }
    }

	public async Task<bool> PostWitnessEvent(ManualWitnessEvent witnessEvent)
    {
		try
		{
			var test = JsonSerializer.Serialize(witnessEvent);
			var result = await this.httpClient.PostAsJsonAsync(Api.WITNESS, witnessEvent);
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
}

public interface IApiService
{
	Task<List<StartModel>> GetStartlist();
	Task<bool> PostWitnessEvent(ManualWitnessEvent witnessEvent);
}
