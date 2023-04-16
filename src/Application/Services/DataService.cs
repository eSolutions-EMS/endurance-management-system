using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Services;

public class DataService : IDataService
{
    private const string HOST = "http://192.168.68.3:11337";
    private const string EVENTS_ENDPOINT = "judge/events";

    private readonly IJsonSerializationService serializationService;

    public DataService(IJsonSerializationService serializationService)
    {
        this.serializationService = serializationService;
    }

    public async Task<Dictionary<int, WitnessEvent>> GetWitnessEvents()
    {
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
}

public interface IDataService : ITransientService
{
    Task<Dictionary<int, WitnessEvent>> GetWitnessEvents();
}
