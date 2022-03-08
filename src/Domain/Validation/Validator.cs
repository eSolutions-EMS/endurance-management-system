using EnduranceJudge.Domain.Core.Exceptions;
using System;
using System.Linq;
using static EnduranceJudge.Localization.Strings.Messages.DomainValidation;
using static EnduranceJudge.Localization.Strings.Words;

namespace EnduranceJudge.Domain.Validation;

public class Validator<T> where T : DomainExceptionBase, new()
{
    public TValue IsRequired<TValue>(TValue obj, string property)
    {
        if (obj?.Equals(default) ?? true)
        {
            throw Helper.Create<T>(IS_REQUIRED_TEMPLATE, property);
        }
        return obj;
    }

    public string IsFullName(string name)
    {
        this.IsRequired(name, NAME);
        var parts = name.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var firstName = parts.FirstOrDefault();
        var lastName = parts.LastOrDefault();
        if (parts.Length < 2 || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            throw Helper.Create<T>(INVALID_FULL_NAME, name);
        }
        return name;
    }

    public void IsLaterThan(DateTime value, DateTime? compareTo, string name)
    {
        if (value <= compareTo)
        {
            throw Helper.Create<T>(DATE_TIME_HAS_TO_BE_LATER_TEMPLATE, name, compareTo);
        }
    }
}
