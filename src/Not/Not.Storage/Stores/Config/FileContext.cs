using Not.Storage.Stores.Files.Ports;
using Not.Storage.Stores.StaticOptions.Ports;

namespace Not.Storage.Stores.Config;

public class FileContext : IFileStorageConfiguration, IStaticOptionsConfiguration
{
    public string? Path { get; set; }

    internal void Validate()
    {
        if (Path == null)
        {
            throw new ApplicationException($"{nameof(FileContext)} is not configured: {nameof(Path)} is required");
        }
    }
}
