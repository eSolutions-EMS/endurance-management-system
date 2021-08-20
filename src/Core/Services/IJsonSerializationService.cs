using EnduranceJudge.Core.ConventionalServices;
using System;

namespace EnduranceJudge.Core.Services
{
    public interface IJsonSerializationService : IService
    {
        string Serialize(object data);

        T Deserialize<T>(string serialized);

        object Deserialize(string serialized, Type resultType);
    }
}
