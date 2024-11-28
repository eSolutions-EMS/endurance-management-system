using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Not.Contexts;
using Not.Injection;
using Not.Storage.Stores.Config;
using NTS.Judge.Shared;

namespace NTS.Judge.Tests;

public abstract partial class JsonFileStoreIntegrationTest : IDisposable
{
    const string ID_PATTERN = @"(?:^\s+|,\s+)""Id"": [0-9]+";
    static Regex _regex = new(ID_PATTERN);
    string _storageFilePath;

    protected JsonFileStoreIntegrationTest(string stateFilename)
    {
        var path = $"{Directory.GetCurrentDirectory()}/{Guid.NewGuid()}";
        Directory.CreateDirectory(path);
        _storageFilePath = $"{path}/{stateFilename}.json";

        var services = ConfigureServices(path);
        Provider = services.BuildServiceProvider();
    }

    protected IServiceProvider Provider { get; private set; }

    protected virtual IServiceCollection ConfigureServices(string storagePath)
    {
        ContextHelper.SetApplicationName("nts");
        var services = new ServiceCollection();
        return services
            .AddJudge()
            .AddJsonFileStore(x => x.Path = storagePath)
            .AddStaticOptionsStore(x => x.Path = ContextHelper.GetAppDirectory("resources"))
            .GetConventionalAssemblies()
            .RegisterConventionalServices();
    }

    protected async Task Seed(string contents)
    {
        await File.WriteAllTextAsync(_storageFilePath, contents);
    }

    protected async Task AssertState(string expectedContents)
    {
        var contents = await File.ReadAllTextAsync(_storageFilePath);
        var expected = Normalize(expectedContents);
        var actual = Normalize(contents);
        Assert.Equal(expected, actual);
    }
    
    public void Dispose()
    {
        File.Delete(_storageFilePath);
    }

    string Normalize(string contents)
    {
        return _regex.Replace(contents, string.Empty);
    }
}
