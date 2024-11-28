using Microsoft.Extensions.DependencyInjection;
using Not.Blazor.CRUD.Ports;
using Not.Serialization;
using NTS.Domain.Objects;
using NTS.Domain.Setup.Aggregates;
using NTS.Judge.Blazor.Setup.EnduranceEvents;
using NTS.Storage.Setup;

namespace NTS.Judge.Tests;

public class UnitTest1 : JsonFileStoreIntegrationTest
{
    [Fact]
    public async void Test1()
    {
        await Seed("{}");
        
        var enduranceEventBehind = Provider.GetRequiredService<ICreateBehind<EnduranceEventFormModel>>();
        
        var country = new Country("testIso", "testNf", "Test");
        var enduranceEvent = new EnduranceEventTestModel
        {
            Country = country,
            Place = "Sofia",
        };
        
        await enduranceEventBehind.Create(enduranceEvent);

        var expectedState = new SetupState
        {
            EnduranceEvent = EnduranceEvent.Update(enduranceEvent.Id, enduranceEvent.Place, enduranceEvent.Country, [], [])
        };

        await AssertState(expectedState.ToJson());
    }
}

public class EnduranceEventTestModel : EnduranceEventFormModel
{
    public new int Id { get; set; }
}
