using EnduranceJudge.Core.ConventionalServices;

namespace EnduranceJudge.Core.Services
{
    public interface IXmlSerializationService : IService
    {
        T Deserialize<T>(string filePath);
    }
}
