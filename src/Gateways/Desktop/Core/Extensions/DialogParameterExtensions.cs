using EnduranceJudge.Gateways.Desktop.Core.Objects;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Desktop.Core.Extensions
{
    public static class DialogParameterExtensions
    {
        private const string TITLE_KEY = "title-key";
        private const string SEVERITY_KEY = "severity-key";
        private const string MESSAGE_KEY = "message-key";

        public static IDialogParameters SetTitle(this IDialogParameters parameters, string name)
        {
            parameters.Add(TITLE_KEY, name);
            return parameters;
        }
        public static string GetTitle(this IDialogParameters parameters)
        {
            var title = parameters.GetValue<string>(TITLE_KEY);
            return title;
        }

        public static IDialogParameters SetSeverity(this IDialogParameters parameters, MessageSeverity severity)
        {
            parameters.Add(SEVERITY_KEY, severity);
            return parameters;
        }
        public static MessageSeverity GetSeverity(this IDialogParameters parameters)
        {
            var severity = parameters.GetValue<MessageSeverity>(SEVERITY_KEY);
            return severity;
        }

        public static IDialogParameters SetMessage(this IDialogParameters parameters, string message)
        {
            parameters.Add(MESSAGE_KEY, message);
            return parameters;
        }
        public static string GetMessage(this IDialogParameters parameters)
        {
            var message = parameters.GetValue<string>(MESSAGE_KEY);
            return message;
        }
    }
}
