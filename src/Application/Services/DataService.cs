using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EnduranceJudge.Application.Services;

public class DataService : IDataService
{
    private const string ENDPOINT = "http://192.168.68.3:11337/judge/events";

    private readonly IJsonSerializationService serializationService;

    public DataService(IJsonSerializationService serializationService)
    {
        this.serializationService = serializationService;
    }

    public Dictionary<int, WitnessEvent> Get()
    {
        using var client = new HttpClient();
        var response = client.GetAsync(ENDPOINT).Result;
        if (!response.IsSuccessStatusCode)
        {
            // TODO: better validation
            throw new Exception($"Failed to fetch Witness events: {response.StatusCode}");
        }
        var body = response.Content.ReadAsStringAsync().Result;
        var state = this.serializationService.Deserialize<Dictionary<int, WitnessEvent>>(body);
        return state;
    }
}

public interface IDataService : ITransientService
{
    Dictionary<int, WitnessEvent> Get();
}
