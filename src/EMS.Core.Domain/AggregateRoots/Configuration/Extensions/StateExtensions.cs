using EMS.Core.Domain.Core.Exceptions;
using EMS.Core.Domain.State;
using EMS.Core.Domain.State.EnduranceEvents;
using static EMS.Core.Localization.Strings;

namespace EMS.Core.Domain.AggregateRoots.Configuration.Extensions;

public static class StateExtensions
{
    public static void ValidateThatEventHasNotStarted(this IState state)
    {
        if (state.Event?.HasStarted ?? false)
        {
            throw Helper.Create<EnduranceEventException>(CHANGE_NOT_ALLOWED_WHEN_EVENT_HAS_STARTED_MESSAGE);
        }
    }
}
