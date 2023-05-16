using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Events;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EnduranceJudge.Domain.AggregateRoots.Manager.WitnessEvents;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static EnduranceJudge.Application.ApplicationConstants;

namespace EnduranceJudge.Application.Services;

public class DataService : IDataService
{
    private const string API_HOST = "http://192.168.68.3:11337";
    private const string EVENTS_ENDPOINT = "judge/events";

    private readonly IJsonSerializationService serializationService;

    public DataService(IJsonSerializationService serializationService)
    {
        this.serializationService = serializationService;
        Witness.StartlistChanged += async (_, startlist)
            => Task.Run(async () => await this.PostStartlist(startlist));
    }

    public async Task<Dictionary<int, WitnessEvent>> GetWitnessEvents()
    {
        //TODO: use IHttpClientFactory
        using var client = new HttpClient();
        var response = await client.GetAsync($"{API_HOST}/{EVENTS_ENDPOINT}");
        if (!response.IsSuccessStatusCode)
        {
            // TODO: better validation
            throw new Exception($"Failed to fetch Witness events: {response.StatusCode}");
        }
        var body = await response.Content.ReadAsStringAsync();
        var state = this.serializationService.Deserialize<Dictionary<int, WitnessEvent>>(body);
        return state;
    }

    public async Task PostStartlist(IEnumerable<StartModel> startlist)
    {
        using var client = new HttpClient();
        try
        {
            var response = await client.PostAsJsonAsync($"{API_HOST}/{Api.STARTLIST}", startlist);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Failed to send Startlist data to API: {response.StatusCode}" +
                    Environment.NewLine +
                    response.Content);
            }
        }
        catch (Exception exception)
        {
            var error  = new Exception("Failed to update Startlist in API", exception);
            CoreEvents.RaiseError(error);
        }
    }
}

public interface IDataService : ISingletonService
{
    Task<Dictionary<int, WitnessEvent>> GetWitnessEvents();
}
