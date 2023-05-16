using EMS.Core.ConventionalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EMS.Core.Localization.Services;

public class StringsPopulator : IStringsPopulator
{
    public void Populate(Type type, Dictionary<string, string> values)
    {
        if (!values?.Any() ?? true)
        {
            throw new Exception("Translation values are empty");
        }
        var fields = type
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .OrderByDescending(x => x.Name.EndsWith("ENTITY"))
            .ThenByDescending(x => x.Name.EndsWith("TERM"));
        foreach (var info in fields)
        {
            var name = info.Name;
            var value = values[name];
            if (value == null)
            {
                throw new Exception($"Missing entry {name} in translations file");
            }
            info.SetValue(null, value);
        }
    }
}

public interface IStringsPopulator : ITransientService
{
    void Populate(Type type, Dictionary<string, string> values);
}
