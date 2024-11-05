using System;
using System.IO;
using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager;
using Core.Services;

namespace EMS.Judge.Application.Services;

public class Logger : ILogger
{
    private readonly IFileService fileService;
    private readonly IPersistence persistence;

    public Logger(IFileService fileService, IPersistence persistence)
    {
        this.fileService = fileService;
        this.persistence = persistence;
    }

    public void Log(string type, string message)
    {
        var filename = $"{DateTime.Now.ToString("dd-hh.mm.ss")}_{type}.txt";
        var path = Path.Combine(this.persistence.StateDirectoryPath, filename);
        this.fileService.Append(path, message);
    }

    public void LogEvent(WitnessEvent witnessEvent)
    {
        var filename = "witness-events.log";
        var now = DateTime.Now;
        var message =
            $"{now}: Event"
            + Environment.NewLine
            + $"  Type: {witnessEvent.Type}"
            + Environment.NewLine
            + $"  RFID: {witnessEvent.TagId}"
            + Environment.NewLine
            + $"  Time: {witnessEvent.Time}"
            + Environment.NewLine;
        this.fileService.Append(this.BuildFilePath(filename), message);
    }

    public void LogEventError(Exception exception)
    {
        var filename = "witness-event-errors.log";
        var now = DateTime.Now;
        var message =
            $"{now}: {exception.Message}"
            + Environment.NewLine
            + exception.StackTrace
            + Environment.NewLine;
        this.fileService.Append(this.BuildFilePath(filename), message);
    }

    private string BuildFilePath(string filename)
    {
        var dir = $"{Directory.GetCurrentDirectory()}/logs";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        return Path.Combine(dir, filename);
    }
}

public interface ILogger : ITransientService
{
    void Log(string type, string message);
    void LogEvent(WitnessEvent witnessEvent);
    void LogEventError(Exception exception);
}
