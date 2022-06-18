using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.ConventionalServices;
using System;
using System.Net.Http;
using System.Text;

namespace EnduranceJudge.Application.Services;

public class DataService : IDataService
{
    private const string ENDPOINT = "http://192.168.0.230:11337/state";
    
    private State cache;
    private DateTime lastRequestAt;
    private readonly IJsonSerializationService serializationService;
    
    public DataService(IJsonSerializationService serializationService)
    {
        this.serializationService = serializationService;
    }

    public State Get()
    {
        if (this.cache == null)
        {
            this.cache = this.Request();
            this.lastRequestAt = DateTime.Now;
        }
        else
        {
            var now = DateTime.Now;
            if (now - this.lastRequestAt > TimeSpan.FromSeconds(1))
            {
                this.cache = this.Request();
            }
        }
        return this.cache;
    }

    private State Request()
    {
        using var client = new HttpClient();
        // var client = this.clientFactory.CreateClient();
        var response = client.GetAsync(ENDPOINT).Result;
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Could not connect to Judge API");
        }
        var body = response.Content.ReadAsStringAsync().Result;
        var state = this.serializationService.Deserialize<State>(body);
        return state;
    }
    
    public void Post(State state)
    {
        using var client = new HttpClient();
        var contents = this.serializationService.Serialize(state);
        var content = new StringContent(contents, Encoding.UTF8, "application/json");

        var response = client.PostAsync(ENDPOINT, content).Result;
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Could save state to Judge API");
        }
    }
}

public interface IDataService : ITransientService
{
    State Get();
    void Post(State state);
}
