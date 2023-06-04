using Microsoft.AspNetCore.Components.Web;

namespace EMS.Witness.Shared;
public class ErrorHandlerComponent : ErrorBoundary
{
    public Exception? Exception => this.CurrentException;
}
