using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EnduranceJudge.Domain.AggregateRoots.Manager.WitnessEvents;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Services;

public class DataService : IDataService
{
    private const string HOST = "http://192.168.68.3:11337";
    private const string EVENTS_ENDPOINT = "judge/events";
    private const string STARTLIST_ENDPOINT = "judge/startlist";

    private readonly IJsonSerializationService serializationService;
    private readonly ManagerRoot managerRoot;

    public DataService(IJsonSerializationService serializationService, ManagerRoot managerRoot)
    {
        this.serializationService = serializationService;
        this.managerRoot = managerRoot;
        Witness.StartlistChanged += async (_, _) => await this.PostStartlist();
    }

    public async Task<Dictionary<int, WitnessEvent>> GetWitnessEvents()
    {
        //TODO: use IHttpClientFactory
        using var client = new HttpClient();
        var response = await client.GetAsync($"{HOST}/{EVENTS_ENDPOINT}");
        if (!response.IsSuccessStatusCode)
        {
            // TODO: better validation
            throw new Exception($"Failed to fetch Witness events: {response.StatusCode}");
        }
        var body = await response.Content.ReadAsStringAsync();
        var state = this.serializationService.Deserialize<Dictionary<int, WitnessEvent>>(body);
        return state;
    }

    public async Task PostStartlist()
    {
        var startlistModels = this.managerRoot.GetStartList(false);
        using var client = new HttpClient();
        var response = await client.PostAsJsonAsync($"{HOST}/{EVENTS_ENDPOINT}", startlistModels);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Failed to send Startlist data to API: {response.StatusCode}" +
                Environment.NewLine +
                response.Content);
        }
    }
}

public interface IDataService : ISingletonService
{
    Task<Dictionary<int, WitnessEvent>> GetWitnessEvents();
}
