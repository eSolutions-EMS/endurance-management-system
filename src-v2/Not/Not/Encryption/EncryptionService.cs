using Not.Injection;

namespace Not.Encryption;

public class EncryptionService : IEncryptionService
{
    readonly IEncryptionSettings _settings;

    public EncryptionService(IEncryptionSettings settings)
    {
        _settings = settings;
    }

    public string Encrypt(string content)
    {
        return content;
    }

    public string Decrypt(string encrypted)
    {
        return encrypted;
    }
}

public interface IEncryptionService : ITransientService
{
    string Encrypt(string content);
    string Decrypt(string encrypted);
}
