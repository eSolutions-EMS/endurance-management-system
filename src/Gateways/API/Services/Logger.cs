using EnduranceJudge.Application.Models;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using System;
using System.IO;

namespace Endurance.Judge.Gateways.API.Services
{
    public class Logger : ILogger
    {
        private const string FILE_NAME = "error.log";
        
        private readonly IFileService fileService;
        public Logger(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public void LogError(Exception exception)
        {
            var now = DateTime.Now;
            var message = 
                $"{now}: " + exception.Message + Environment.NewLine
                + exception.StackTrace + Environment.NewLine;
            
            this.Log(message);
        }

        // TODO: remove
        public void LogEventError(Exception exception, WitnessEvent witnessEvent)
        {
            var now = DateTime.Now;
            var message =
                $"{now}: Error while executing Witness Event" + Environment.NewLine +
                $"Event: {witnessEvent.Type}" + Environment.NewLine +
                $"TagId: {witnessEvent.TagId}" + Environment.NewLine +
                $"TimeStamp: {witnessEvent.Time}" + Environment.NewLine +
                exception.Message + Environment.NewLine +
                exception.StackTrace + Environment.NewLine;

            this.Log(message);
        }

        private void Log(string message)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), FILE_NAME);
            this.fileService.Append(path, message);
        }
    }
}

public interface ILogger : ITransientService
{
    void LogError(Exception exception);
    void LogEventError(Exception exception, WitnessEvent witnessEvent);
}
