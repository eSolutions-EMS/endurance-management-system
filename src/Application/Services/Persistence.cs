using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Application.Models;
using EnduranceJudge.Application.State;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Domain.AggregateRoots.Manager.WitnessEvents;
using EnduranceJudge.Domain.State;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Services;

public class Persistence : IPersistence
{
    private const string STORAGE_FILE_NAME = "judge.json";
    private const string SANDBOX_STORAGE_FILE_NAME = "judge-sandbox.json";
    private string stateDirectoryPath;

    private readonly ISettings settings;
    private readonly IStateSetter stateSetter;
    private readonly IState state;
    private readonly IFileService file;
    private readonly IJsonSerializationService serialization;

    public Persistence(
        ISettings settings,
        IStateContext context,
        IStateSetter stateSetter,
        IFileService file,
        IJsonSerializationService serialization)
    {
        this.settings = settings;
        this.stateSetter = stateSetter;
        this.state = context.State;
        this.file = file;
        this.serialization = serialization;
        Witness.StateChanged += (_, _) => this.SaveState();
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
        var path = $"{this.stateDirectoryPath}/{filename}";
        this.file.Create(path, serialized);
        return path;
    }

    public PersistenceResult Configure(string directoryPath)
    {
        this.stateDirectoryPath = directoryPath;
        var database = this.GetFilePath();
        if (this.file.Exists(database))
        {
            var sandboxDatabase = this.GetSandBoxFilePath();
            var path = this.settings.IsSandboxMode &&this.file.Exists(sandboxDatabase)
                ? sandboxDatabase
                : database;
            this.SetState(path);
            return PersistenceResult.Existing;
        }

        this.SaveState();
        return PersistenceResult.New;
    }

    public void SaveState()
    {
        var serialized = this.serialization.Serialize(this.state);
        var databasePath = BuildStorageFilePath();
        this.file.Create(databasePath, serialized);
    }

    private void SetState(string path)
    {
        var contents = this.file.Read(path);
        var state = this.serialization.Deserialize<StateModel>(contents);
        this.stateSetter.Set(state);
    }

    private string BuildStorageFilePath()
        => this.settings.IsSandboxMode
            ? this.GetSandBoxFilePath()
            : this.GetFilePath();

    private string GetSandBoxFilePath()
        => $"{this.stateDirectoryPath}\\{SANDBOX_STORAGE_FILE_NAME}";
    
    private string GetFilePath()
        => $"{this.stateDirectoryPath}\\{STORAGE_FILE_NAME}";
}


public interface IPersistence : ISingletonService
{
    void SaveState();
    string LogError(string message, string stackTrace);
    PersistenceResult Configure(string directoryPath);
}
