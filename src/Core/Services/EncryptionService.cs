using System;
using Core.ConventionalServices;

namespace Core.Services;

public class EncryptionService : IEncryptionService
{
    private const string KEY = "E546C8DF278CD5931069B522E695D4F2";

    public EncryptionService() { }

    public string Encrypt(string content)
    {
        throw new NotImplementedException();
    }

    public string Decrypt(string encrypted)
    {
        throw new NotImplementedException();
    }
}

public interface IEncryptionService : ITransientService
{
    string Encrypt(string content);
    string Decrypt(string encrypted);
}
