using EMS.Core.ConventionalServices;
using System;

namespace EMS.Core.Services;

public class EncryptionService : IEncryptionService
{
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
