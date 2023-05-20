using Core.Domain.Common.Exceptions;
using Core.Domain.State;
using Core.Domain.State.EnduranceEvents;
using static Core.Localization.Strings;

namespace Core.Domain.AggregateRoots.Configuration.Extensions;

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
