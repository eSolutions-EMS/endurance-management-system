﻿using EnduranceJudge.Application.Core.Exceptions;
using EnduranceJudge.Core.ConventionalServices;
using System;
using System.Globalization;
using static EnduranceJudge.Localization.Strings.Application;

namespace EnduranceJudge.Application.Core.Services
{
    public class DateService : IDateService
    {
        public DateTime Parse(string date, string format)
        {
            var hasParsed = DateTime.TryParseExact(
                date,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var result);
            if (!hasParsed)
            {
                var message = string.Format(INVALID_DATE_FORMAT, date, format);
                throw new AppException(message);
            }
            return result;
        }
    }

    public interface IDateService : IService
    {
        DateTime Parse(string date, string format);

    }
}
