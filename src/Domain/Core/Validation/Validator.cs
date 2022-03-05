using EnduranceJudge.Domain.Core.Exceptions;
using System;
using System.Linq;
using static EnduranceJudge.Localization.Translations.Messages.DomainValidation;
using static EnduranceJudge.Localization.Translations.Words;

namespace EnduranceJudge.Domain.Core.Validation;

//TODO: move out of Core
public class Validator<T> where T : DomainExceptionBase, new()
{
    public TValue IsRequired<TValue>(TValue obj, string property)
    {
        if (obj?.Equals(default) ?? true)
        {
            throw DomainExceptionBase.Create<T>(IS_REQUIRED_TEMPLATE, property);
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
            throw DomainExceptionBase.Create<T>(INVALID_FULL_NAME, name);
        }
        return name;
    }
}
