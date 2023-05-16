using EMS.Core.ConventionalServices;
using Microsoft.AspNetCore.DataProtection;

namespace EMS.Core.Services;

public class EncryptionService : IEncryptionService
{
    private const string KEY = "E546C8DF278CD5931069B522E695D4F2";

    public EncryptionService()
    {
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
