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

    private readonly IState appState;
    private readonly IFileService file;
    private readonly IJsonSerializationService serialization;
    
    public Persistence(IState appState, IFileService file, IJsonSerializationService serialization)
    {
        this.appState = appState;
        this.file = file;
        this.serialization = serialization;
    }
    
    public void Snapshot()
    {
        // TODO: call API
    }

    public string LogError(string message, string stackTrace)
    {
        var timestamp = DateTime.Now.ToString("yyyy-mm-ddTHH-mm-ss");
        var log = new Dictionary<string, object>
        {
            { "error-message", message },
            { "error-stack-trance", stackTrace },
            { "state", this.appState },
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
        // Call API
        return PersistenceResult.New;
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
}


public interface IPersistence : ITransientService
{
    void Snapshot();
    string LogError(string message, string stackTrace);
    PersistenceResult Initialize(string directoryPath);
}
