using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.EnduranceEvents;
using static EnduranceJudge.Localization.Strings.Messages.DomainValidation;

namespace EnduranceJudge.Domain.AggregateRoots.Configuration.Extensions;

public static class StateExtensions
{
    public static void ValidateThatEventHasNotStarted(this IState state)
    {
        if (state.Event.HasStarted)
        {
            throw Helper.Create<EnduranceEventException>(CHANGE_NOT_ALLOWED_WHEN_EVENT_HAS_STARTED_MESSAGE);
        }
    }
}
