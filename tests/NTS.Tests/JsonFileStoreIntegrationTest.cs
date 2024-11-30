using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Not.Contexts;
using Not.Injection;
using Not.Storage.Stores;
using NTS.Judge.Shared;

namespace NTS.Judge.Tests;

public abstract partial class JsonFileStoreIntegrationTest : IDisposable
{
    const string ID_PATTERN = @"(?:^\s+|,\s+)""Id"": [0-9]+";
    static Regex _regex = new(ID_PATTERN);
    string _storageFilePath;
    string _storageDirectory;

    protected JsonFileStoreIntegrationTest(string stateFilename)
    {
        _storageDirectory = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString());
        _storageFilePath = $"{_storageDirectory}/{stateFilename}.json";
        Directory.CreateDirectory(_storageDirectory);
        
        var services = ConfigureServices(_storageDirectory);
        Provider = services.BuildServiceProvider();
    }

    protected IServiceProvider Provider { get; private set; }

    protected virtual IServiceCollection ConfigureServices(string storagePath)
    {
        FileContextHelper.SetRootDirectory("nts");
        var services = new ServiceCollection();
        return services
            .AddJudge()
            .AddJsonFileStore(x => x.Path = storagePath)
            .AddStaticOptionsStore(x => x.Path = FileContextHelper.GetAppDirectory("resources"))
            .GetConventionalAssemblies()
            .RegisterConventionalServices();
    }

    protected async Task Seed(string contents)
    {
        await File.WriteAllTextAsync(_storageFilePath, contents);
    }

    protected async Task SeedResource(string resourcefile)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), $"Resources", resourcefile);
        var contents = await File.ReadAllTextAsync(path);
        await File.WriteAllTextAsync(_storageFilePath, contents);
    }

    protected async Task AssertStateEquals(string expectedContents)
    {
        var contents = await File.ReadAllTextAsync(_storageFilePath);
        var expected = Normalize(expectedContents);
        var actual = Normalize(contents);
        Assert.Equal(expected, actual, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
    }
    
    public void Dispose()
    {
        File.Delete(_storageFilePath);
        Directory.Delete(_storageDirectory);
    }

    string Normalize(string contents)
    {
        return _regex.Replace(contents, string.Empty);
    }
}
