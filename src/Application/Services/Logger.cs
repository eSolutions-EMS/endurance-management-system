using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using System;
using System.IO;

namespace EnduranceJudge.Application.Services;

public class Logger : ILogger
{
    private readonly IFileService fileService;
    public Logger(IFileService fileService)
    {
        this.fileService = fileService;
    }
    
    public void LogEvent(WitnessEvent witnessEvent)
    {
        var filename = "witness-events.log";
        var now = DateTime.Now;
        var message = $"{now}: type-{witnessEvent.Type} rfid-{witnessEvent.TagId} time-{witnessEvent.Time}";
        this.fileService.Append(this.BuildFilePath(filename), message);
    }

    public void LogEventError(Exception exception)
    {
        var filename = "witness-event-errors.log";
        var now = DateTime.Now;
        var message =
            $"{now}: {exception.Message}" + Environment.NewLine +
            exception.StackTrace + Environment.NewLine;
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
    void LogEvent(WitnessEvent witnessEvent);
    void LogEventError(Exception exception);
}
