using EnduranceJudge.Core.ConventionalServices;
using System.Xml.Serialization;

namespace EnduranceJudge.Core.Services;

public class XmlSerializationService : IXmlSerializationService
{
    private readonly IFileService file;

    public XmlSerializationService(IFileService file)
    {
        this.file = file;
    }

    public T Deserialize<T>(string filePath)
    {
        var serializer = new XmlSerializer(typeof(T));

        using var readStream = this.file.ReadStream(filePath);
        var data = (T)serializer.Deserialize(readStream)!;

        return data;
    }
}

public interface IXmlSerializationService : ITransientService
{
    T Deserialize<T>(string filePath);
}
