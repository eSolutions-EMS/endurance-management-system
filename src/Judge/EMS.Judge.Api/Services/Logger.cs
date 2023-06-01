using Core.ConventionalServices;
using Core.Services;
using Core.Domain.AggregateRoots.Manager;
using System;
using System.IO;

namespace EMS.Judge.Api.Services
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

        public void LogRpcError(Exception exception)
        {
            var now = DateTime.Now;
            var message = 
                $"{now}: " + exception.Message + Environment.NewLine
                + exception.StackTrace + Environment.NewLine;
            var filename = $"T{now:HH-mm-ss}_rpc-error.log";
            this.Log(message, filename);
        }

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

        private void Log(string message, string filename = FILE_NAME)
        {
            var dir = $"{Directory.GetCurrentDirectory()}/logs";
            var path = Path.Combine(dir, filename);
            this.fileService.Append(path, message);
        }
    }
}

public interface ILogger : ITransientService
{
    void LogError(Exception exception);
    void LogRpcError(Exception exception);
    void LogEventError(Exception exception, WitnessEvent witnessEvent);
}
