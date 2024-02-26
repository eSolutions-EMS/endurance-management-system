using Not.Injection;

namespace Not.Encryption;

public interface IEncryptionSettings : ISingletonService
{
    public string Key { get; }
}
