using EnduranceJudge.Application.Import.Contracts;
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
        private static readonly Type DbContextType = typeof(EnduranceJudgeDbContext);

        private readonly ISeederService seeder;
        private readonly IEncryptionService encryption;
        private readonly IFileService file;
        private readonly IJsonSerializationService serialization;
        private readonly EnduranceJudgeDbContext dbContext;

        public WorkFileService(
            ISeederService seeder,
            IEncryptionService encryption,
            IFileService file,
            IJsonSerializationService serialization,
            EnduranceJudgeDbContext dbContext)
        {
            this.seeder = seeder;
            this.encryption = encryption;
            this.file = file;
            this.serialization = serialization;
            this.dbContext = dbContext;
        }

        public async Task<bool> Initialize(string path, CancellationToken cancellationToken)
        {
            workFilePath = path;

            if (this.file.Exists(path))
            {
                await this.RestoreFrom(path);
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
            var dbSetProperties = this.GetEntitySets();

            var data = dbSetProperties.ToDictionary(
                propertyInfo => propertyInfo.Name,
                propertyInfo =>
                {
                    var entities = propertyInfo.GetValue(this.dbContext);
                    return this.serialization.Serialize(entities!);
                });

            var serialized = this.serialization.Serialize(data);
            var encrypted = this.encryption.Encrypt(serialized);

            await this.file.Create(filePath, encrypted);
        }

        private async Task RestoreFrom(string filePath)
        {
            var encrypted = await this.file.Read(filePath);
            var decrypted = this.encryption.Decrypt(encrypted);
            var deserialized = this.serialization.Deserialize<Dictionary<string, string>>(decrypted);

            var dbSetProperties = this.GetEntitySets();

            foreach (var (setName, serializedSet) in deserialized)
            {
                var entitySet = dbSetProperties.FirstOrDefault(property => property.Name == setName);
                if (entitySet == null)
                {
                    continue;
                }

                this.CallDbSetAddRange(setName, serializedSet, entitySet);
            }

            await this.dbContext.SaveChangesAsync();
        }

        private IList<PropertyInfo> GetEntitySets()
        {
            var properties = ReflectionUtilities.GetProperties<EnduranceJudgeDbContext>(
                BindingFlags.Public
                | BindingFlags.Instance);

            var dbSetProperties = properties
                .Where(propertyInfo =>
                    propertyInfo.PropertyType.IsGenericType &&
                    (propertyInfo.PropertyType.BaseType?.IsAssignableFrom(PersistenceConstants.Types.DbSet) ?? false))
                .ToList();

            return dbSetProperties;
        }

        private void CallDbSetAddRange(string dbSetName, string serializedEntityCollection, PropertyInfo dbSet)
        {
            var entityType = dbSet.PropertyType
                .GetGenericArguments()
                .First();

            var entityCollectionType = PersistenceConstants.Types.List.MakeGenericType(entityType);
            var entityCollection = this.serialization.Deserialize(serializedEntityCollection, entityCollectionType);

            var entitySetType = PersistenceConstants.Types.DbSet.MakeGenericType(entityType);
            var addRangeMethod = ReflectionUtilities.GetMethod(entitySetType, "AddRange", entityCollectionType);

            // Expression
            // db =>
            var dbContextParam = Expression.Parameter(DbContextType, "dbContext");

            // db => db.<DbSet property>
            var dbSetAccessor = Expression.Property(dbContextParam, dbSetName);

            // entities
            var entityCollectionParam = Expression.Parameter(entityCollectionType, "entities");

            // db => db.SetName.AddRange(entities)
            var call = Expression.Call(dbSetAccessor, addRangeMethod, entityCollectionParam);

            var lambdaType = PersistenceConstants.Types.Action.MakeGenericType(DbContextType, entityCollectionType);
            var lambda = Expression.Lambda(lambdaType, call, dbContextParam, entityCollectionParam);

            lambda.Compile().DynamicInvoke(this.dbContext, entityCollection);
        }
    }
}
