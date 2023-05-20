using System.Collections.Generic;

namespace EMS.Judge.Common;

public readonly struct NavigationParameter
{
    public NavigationParameter(string key, object value)
    {
        this.Pair = KeyValuePair.Create(key, value);
    }

    public KeyValuePair<string, object> Pair { get; }

    public void Deconstruct(out string key, out object value)
    {
        key = this.Pair.Key;
        value = this.Pair.Value;
    }
}
