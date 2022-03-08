using EnduranceJudge.Core.ConventionalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnduranceJudge.Localization.Services;

public class Populator : IPopulator
{
    public void Populate(Type type, Dictionary<string, string> values)
    {
        if (!values?.Any() ?? true)
        {
            throw new Exception("Translation values are empty");
        }
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
        foreach (var info in fields)
        {
            var name = info.Name;
            var value = values[name];
            if (value == null)
            {
                continue;
                // throw new Exception($"Missing entry {name} in translations file");
            }
            var processed = Replace(value);
            info.SetValue(null, processed);
        }
    }

    private static string Replace(string template)
    {
        foreach (var (placeholder, value) in LocalizationConstants.PLACEHOLDERS_VALUES)
        {
            template = template.Replace(placeholder, value);
        }
        return template;
    }
}

public interface IPopulator : IService
{
    void Populate(Type type, Dictionary<string, string> values);
}
