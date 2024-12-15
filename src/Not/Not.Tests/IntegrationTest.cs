using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Not.Blazor.Ports;
using Not.Tests.RPC;
using Xunit;

namespace Not.Tests;

public abstract class IntegrationTest : IDisposable
{
    const string SEED_DIRECTORY = "Seeds";
    const string ID_PATTERN = @"(?:^\s+|,\s+)""Id"": [0-9]+";
    static readonly TimeSpan RPC_DELAY = TimeSpan.FromMilliseconds(100);
    static readonly Regex ID_REGEX = new(ID_PATTERN);
    static readonly SemaphoreSlim SEMAPHORE = new(1);

    string _stateName;
    string _storageFilePath;
    string _storageDirectory;
    string _testClassName;

    protected IntegrationTest(string stateName)
    {
        var executionDirectory = Directory.GetCurrentDirectory();
        var tempDirectory = Guid.NewGuid().ToString();

        _stateName = stateName;
        _storageDirectory = Path.Combine(executionDirectory, tempDirectory);
        _storageFilePath = $"{_storageDirectory}/{stateName}.json";
        _testClassName = GetType().Name;

        Directory.CreateDirectory(_storageDirectory);

        var services = ConfigureServices(_storageDirectory);
        Provider = services.BuildServiceProvider();
    }

    protected abstract IServiceCollection ConfigureServices(string storagePath);

    protected IServiceProvider Provider { get; private set; }

    protected async Task<T> GetBehind<T>()
        where T : notnull
    {
        var behind = Provider.GetRequiredService<T>();
        if (behind is IObservableBehind observableBehind)
        {
            await observableBehind.Initialize([]);
        }
        return behind;
    }

    protected async Task Seed([CallerMemberName] string? test = null)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var path = Path.Combine(currentDirectory, SEED_DIRECTORY, _stateName, _testClassName, $"{test}.json");
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

    public async Task AssertRpcInvoked<T>(HubFixture<T> fixture, Func<Task> action, string rpcName)
        where T : ITestRpcClient
    {
        await SEMAPHORE.WaitAsync(millisecondsTimeout: 500);

        var client = fixture.GetClient();
        await client.Connect();
        await action();
        await Task.Delay(RPC_DELAY); //TODO: a more sophisticated method maybe necessary with a lot of tests

        Assert.Contains(rpcName, client.InvokedMethods);
        SEMAPHORE.Release();
    }

    public void Dispose()
    {
        File.Delete(_storageFilePath);
        Directory.Delete(_storageDirectory);
    }

    string Normalize(string contents)
    {
        return ID_REGEX.Replace(contents, string.Empty);
    }
}
