using Core.ConventionalServices;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core.Application.Services;

public class HandshakeValidatorService : IHandshakeValidatorService
{
    // TODO: move key in the secrets repo
    private const string KEY = "psst-secret-dont-tell-anyone";
    private HMACSHA256 _sha;

    public HandshakeValidatorService()
    {
        this._sha = new HMACSHA256(this.GetBytes(KEY));
    }

    public byte[] CreatePayload(string thisApp)
    {
        using var stream = new MemoryStream(this.GetBytes(thisApp));
        var result = this._sha.ComputeHash(stream);
        return result;
    }

    public bool ValidatePayload(byte[] payload, string remoteApp)
    {
        using var stream = new MemoryStream(this.GetBytes(remoteApp));
        var expectedPayload = this._sha.ComputeHash(stream);
        return this.Compare(payload, expectedPayload);
    }

    private bool Compare(byte[] first, byte[] second)
    {
        if (first.Length != second.Length)
            return false;
        for (var i = 0; i < first.Length; i++)
            if (first[i] != second[i])
                return false;
        return true;
    }

    private byte[] GetBytes(string content)
        => Encoding.UTF8.GetBytes(content);
}

public interface IHandshakeValidatorService : ITransientService
{
    byte[] CreatePayload(string thisApp);
    bool ValidatePayload(byte[] payload, string remoteApp);
}
