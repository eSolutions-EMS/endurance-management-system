using EnduranceJudge.Application;
using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Services;
using System.IO;

namespace Endurance.Judge.Gateways.API.Services
{
    public class StateManager : IStateManager
    {
        private const string DATA_FILE = "judge.data";

        private readonly IFileService fileService;
        private readonly IJsonSerializationService serializationService;
        private readonly Context context;

        public StateManager(IFileService fileService, IJsonSerializationService serializationService, Context context)
        {
            this.fileService = fileService;
            this.serializationService = serializationService;
            this.context = context;
        }

        public void Set(State update)
        {
            this.context.State = update;
            update.MapFrom(this.context.State);
            this.context.State = update;
            
            this.Persist();
        }
        
        private void Persist()
        {
            var serialized = this.serializationService.Serialize(this.context.State);
            this.fileService.Create(this.GetDateFilePath(), serialized);
        }

        private string GetDateFilePath()
            => Path.Combine(Directory.GetCurrentDirectory(), DATA_FILE);
    }

    public interface IStateManager : ISingletonService
    {
        void Set(State state);
    }
}
