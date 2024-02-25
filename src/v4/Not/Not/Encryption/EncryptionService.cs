using Not.Conventions;
using System;

namespace Not.Encryption;

public class EncryptionService : IEncryptionService
{
    private readonly IEncryptionSettings settings;

    public EncryptionService(IEncryptionSettings settings)
    {
        this.settings = settings;
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
