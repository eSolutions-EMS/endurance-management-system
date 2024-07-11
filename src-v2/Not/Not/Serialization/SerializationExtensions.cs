using JsonNet.PrivatePropertySetterResolver;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Not.Serialization;

public static class SerializationExtensions
{
    private static JsonSerializerSettings _settings = new()
    {
        ContractResolver = new Kur(),
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
    };

    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj, _settings);
    }

    public static T FromJson<T>(this string json)
        where T : class
    {
        var result = JsonConvert.DeserializeObject<T>(json, _settings);
        if (result == default)
        {
            throw new Exception($"Cannot serialize '{json}' to type of '{typeof(T)}'");
        }
        return result;
    }
}

public class Kur : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);
        if (!jsonProperty.Writable && member is PropertyInfo propertyInfo)
        {
            bool writable = (object)propertyInfo.SetMethod != null;
            jsonProperty.Writable = writable;
        }

        return jsonProperty;
    }
}
