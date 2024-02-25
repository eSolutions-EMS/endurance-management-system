using Common.Conventions;

namespace Common.Encryption;

public interface IEncryptionSettings : ISingletonService
{
    public string Key { get; }
}
