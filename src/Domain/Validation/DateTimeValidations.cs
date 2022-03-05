using EnduranceJudge.Domain.Core.Exceptions;
using System;
using static EnduranceJudge.Localization.Translations.Messages;

namespace EnduranceJudge.Domain.Validation
{
    public static class DateTimeValidations
    {
        public static DateTime IsFutureDate<T>(this DateTime dateTime)
            where T : DomainExceptionBase, new()
        {
            if (dateTime <= DateTime.Now)
            {
                throw DomainExceptionBase.Create<T>(INVALID_FUTURE_DATE_TEMPLATE, dateTime);
            }

            return dateTime;
        }
    }
}
