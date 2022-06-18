using EnduranceJudge.Application.Services;
using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Domain;
using EnduranceJudge.Domain.AggregateRoots.Configuration;
using EnduranceJudge.Domain.State;
using System;
using System.Text.RegularExpressions;

namespace EnduranceJudge.Application.Models;

public class Storage : IStorage
{
    private string dirPath;
    private readonly State appState;
    private readonly IEncryptionService encryption;
    private readonly IFileService file;
    private readonly IJsonSerializationService serialization;

    public Storage(
        State appState,
        IEncryptionService encryption,
        IFileService file,
        IJsonSerializationService serialization)
    {
        this.appState = appState;
        this.encryption = encryption;
        this.file = file;
        this.serialization = serialization;
    }

    public PersistenceResult Initialize(string directoryPath)
    {
        this.dirPath = directoryPath;
        var database = BuildStorageFilePath(directoryPath);
        if (this.file.Exists(database))
        {
            this.Restore();
            return PersistenceResult.Existing;
        }
        else
        {
            this.Create();
            return PersistenceResult.New;
        }
    }

    private void Restore()
    {
        var dataPath = BuildStorageFilePath(this.dirPath);
        var contents = this.file.Read(dataPath);
        contents = this.NormalizeStorageFileContents(contents);
        var state = this.serialization.Deserialize<State>(contents);
        ReferenceNormalizer.Normalize(state);
        this.FixDatesForToday(state);
        // this.__REVERT_START_PARTICIPATIONS__();
        this.appState.Restore(state);
        ;
    }

    private void Create()
    {
        var serialized = this.serialization.Serialize(this.appState);
        var databasePath = BuildStorageFilePath(this.dirPath);
        this.file.Create(databasePath, serialized);
    }

    // TODO: Remove after testing lap
    private string NormalizeStorageFileContents(string contents)
    {
        // Normalize countries due to change in code
        var correctCountryData = "Country\":{\"IsoCode\":\"BGR\",\"Name\":\"Bulgaria\",\"Id\":1}";
        var regex = new Regex("Country\":{\"IsoCode\":\"BUL\",\"Name\":\"Bulgaria\",\"Id\":[0-9]*}");
        return regex.Replace(contents, correctCountryData);
    }

    // TODO: Remove after testing lap
    private void FixDatesForToday(IState state)
    {
        foreach (var competition in state.Event.Competitions)
        {
            competition.StartTime = FixDateForToday(competition.StartTime);
        }
        foreach (var participant in state.Participants)
        {
            foreach (var performance in participant.LapRecords)
            {
                performance.StartTime = FixDateForToday(performance.StartTime);
                if (performance.ArrivalTime.HasValue)
                {
                    performance.ArrivalTime = FixDateForToday(performance.ArrivalTime.Value);
                }
                if (performance.InspectionTime.HasValue)
                {
                    performance.InspectionTime = FixDateForToday(performance.InspectionTime.Value);
                }
                if (performance.ReInspectionTime.HasValue)
                {
                    performance.ReInspectionTime = FixDateForToday(performance.ReInspectionTime.Value);
                }
            }
        }
    }

    private DateTime FixDateForToday(DateTime date)
    {
        var today = DateTime.Today;
        today = today.AddHours(date.Hour);
        today = today.AddMinutes(date.Minute);
        today = today.AddSeconds(date.Second);
        today = today.AddMilliseconds(date.Millisecond);
        return today;
    }

    // TODO: Remove after testing lap
    private void __REVERT_START_PARTICIPATIONS__()
    {
        var manager = new ConfigurationRoot();
        manager.__REVERT_START_PARTICIPATIONS__();
    }
}

public interface IStorage : IStorageInitializer, ISingletonService
{
}
