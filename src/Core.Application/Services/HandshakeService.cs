using Core.ConventionalServices;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core.Application.Services;

public class HandshakeService : IHandshakeService
{
    // TODO: move key in the secrets repo
    private const string KEY = "psst-secret-dont-tell-anyone";
    private HMACSHA256 _sha;

    public HandshakeService()
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
        return payload == expectedPayload;
    }

    private byte[] GetBytes(string content)
        => Encoding.UTF8.GetBytes(content);
}

public interface IHandshakeService : ITransientService
{
    byte[] CreatePayload(string thisApp);
    bool ValidatePayload(byte[] payload, string remoteApp);
}
