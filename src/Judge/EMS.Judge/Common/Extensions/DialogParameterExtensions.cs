using EMS.Judge.Common.Objects;
using Prism.Services.Dialogs;

namespace EMS.Judge.Common.Extensions;

public static class DialogParameterExtensions
{
    private const string SEVERITY_KEY = "severity-key";
    private const string MESSAGE_KEY = "message-key";
    private const string START_LIST_KEY = "start-list-key";

    public static IDialogParameters SetSeverity(
        this IDialogParameters parameters,
        MessageSeverity severity
    )
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
