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

        public void Load()
        {
            var path = this.GetDataFilePath();
            if (File.Exists(path))
            {
                var contents = this.fileService.Read(path);
                this.context.State = this.serializationService.Deserialize<State>(contents);
            }
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
            this.fileService.Create(this.GetDataFilePath(), serialized);
        }

        private string GetDataFilePath()
            => Path.Combine(Directory.GetCurrentDirectory(), DATA_FILE);
    }

    public interface IStateManager : ISingletonService
    {
        void Load();
        void Set(State state);
    }
}
