using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Application.Models;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Domain.State;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Services;

public class Persistence : IPersistence
{
    private static string dataDirectoryPath;

    private readonly IDataService dataService;
    private readonly IState state;
    private readonly IFileService file;
    private readonly IJsonSerializationService serialization;
    
    public Persistence(
        IDataService dataService,
        IStateContext context,
        IFileService file,
        IJsonSerializationService serialization)
    {
        this.dataService = dataService;
        this.state = context.State;
        this.file = file;
        this.serialization = serialization;
    }
    
    public void Snapshot()
    {
        this.dataService.Post(this.state);
    }

    public string LogError(string message, string stackTrace)
    {
        var timestamp = DateTime.Now.ToString("yyyy-mm-ddTHH-mm-ss");
        var log = new Dictionary<string, object>
        {
            { "error-message", message },
            { "error-stack-trance", stackTrace },
            { "state", this.state },
        };
        var serialized = this.serialization.Serialize(log);
        var filename = $"{timestamp}_error.json";
        var path = $"{dataDirectoryPath}/{filename}";
        this.file.Create(path, serialized);
        return path;
    }
    
    public PersistenceResult Initialize(string directoryPath)
    {
        if (dataDirectoryPath is not null)
        {
            throw new Exception("Application data configuration is already initialized");
        }
        dataDirectoryPath = directoryPath;
        
        return PersistenceResult.New;
    }
}


public interface IPersistence : ITransientService
{
    void Snapshot();
    string LogError(string message, string stackTrace);
    PersistenceResult Initialize(string directoryPath);
}
