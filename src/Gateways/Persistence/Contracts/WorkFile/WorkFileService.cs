using EnduranceJudge.Application.Imports.Contracts;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Gateways.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Persistence.Contracts.WorkFile
{
    public class WorkFileService : IWorkFileService, IWorkFileUpdater, ISingletonService
    {
        private static string workFilePath;

        private readonly ISeederService seeder;
        private readonly IEncryptionService encryption;
        private readonly IFileService file;
        private readonly IJsonSerializationService serialization;

        public WorkFileService(
            ISeederService seeder,
            IEncryptionService encryption,
            IFileService file,
            IJsonSerializationService serialization)
        {
            this.seeder = seeder;
            this.encryption = encryption;
            this.file = file;
            this.serialization = serialization;
        }

        public async Task<bool> Initialize(string path, CancellationToken cancellationToken)
        {
            workFilePath = path;

            if (this.file.Exists(path))
            {
                // await this.RestoreFrom(path);
                return false;
            }

            await this.seeder.Seed();
            await this.Create(path);

            return true;
        }

        public Task Snapshot()
            => this.Create(workFilePath);

        private async Task Create(string filePath)
        {
            // var dbSetProperties = this.GetEntitySets();
            //
            // var data = dbSetProperties.ToDictionary(
            //     propertyInfo => propertyInfo.Name,
            //     propertyInfo =>
            //     {
            //         var entities = propertyInfo.GetValue(this.dbContext);
            //         return this.serialization.Serialize(entities!);
            //     });
            //
            // var serialized = this.serialization.Serialize(data);
            // var encrypted = this.encryption.Encrypt(serialized);
            //
            // await this.file.Create(filePath, encrypted);
        }
    }
}
