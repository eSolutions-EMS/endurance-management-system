using Microsoft.AspNetCore.DataProtection;

namespace EnduranceJudge.Core.Services.Implementations
{
    public class EncryptionService : IEncryptionService
    {
        private const string Key = "E546C8DF278CD5931069B522E695D4F2";
        private readonly IDataProtector dataProtector;

        public EncryptionService(IDataProtectionProvider dataProtectionProvider)
        {
            this.dataProtector = dataProtectionProvider.CreateProtector(Key);
        }

        public string Encrypt(string content)
        {
            var encrypted = this.dataProtector.Protect(content);
            return encrypted;
        }

        public string Decrypt(string encrypted)
        {
            var decrypted = this.dataProtector.Unprotect(encrypted);
            return decrypted;
        }
    }
}
