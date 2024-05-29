using Core.ConventionalServices;
using System.IO;
using System.Xml.Serialization;

namespace Core.Services;

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

    public void SerializeToFile<T>(T data, string path)
    {
        var serializer = new XmlSerializer(typeof(T));
        serializer.Serialize(File.Create(path), data);
    }
}

public interface IXmlSerializationService : ITransientService
{
    T Deserialize<T>(string filePath);
    void SerializeToFile<T>(T data, string path);
}
