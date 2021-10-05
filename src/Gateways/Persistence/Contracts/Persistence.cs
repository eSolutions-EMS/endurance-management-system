using EnduranceJudge.Application.Imports.Contracts;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Gateways.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Persistence.Contracts
{
    public class Persistence : IPersistence
    {
        private string storageFilePath;

        private readonly IDataContext dataContext;
        private readonly IEncryptionService encryption;
        private readonly IFileService file;
        private readonly IJsonSerializationService serialization;

        public Persistence(
            IDataContext dataContext,
            IEncryptionService encryption,
            IFileService file,
            IJsonSerializationService serialization)
        {
            this.dataContext = dataContext;
            this.encryption = encryption;
            this.file = file;
            this.serialization = serialization;
        }

        public async Task<bool> Initialize(string path, CancellationToken token)
        {
            this.storageFilePath = path;

            if (this.file.Exists(path))
            {
                await this.Restore(path);
                return false;
            }
            else
            {
                await this.Create();
                return true;
            }
        }

        public async Task Snapshot() => await this.Create();

        private async Task Restore(string path)
        {
            var contents = await this.file.Read(path);
            var state = this.serialization.Deserialize<State>(contents);
            this.dataContext.State.MapFrom(state);
        }

        private async Task Create()
        {
            var serialized = this.serialization.Serialize(this.dataContext.State);
            await this.file.Create(this.storageFilePath, serialized);
        }
    }

    public interface IPersistence : IStorageInitializer, ISingletonService
    {
        Task Snapshot();
    }
}
