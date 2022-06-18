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
        
        public string GetRaw()
        {
            var path = this.GetDateFilePath();
            var contents = this.fileService.Read(path);
            return contents;
        }
        
        public State Get()
        {
            var contents = this.GetRaw();
            var state = this.serializationService.Deserialize<State>(contents);
            return state;
        }

        public void Update(State update)
        {
            var current = this.Get();
            current.MapFrom(update);
            this.context.State = current;
            
            var serialized = this.serializationService.Serialize(current);
            this.fileService.Create(this.GetDateFilePath(), serialized);
        }

        private string GetDateFilePath()
            => Path.Combine(Directory.GetCurrentDirectory(), DATA_FILE);
    }

    public interface IStateManager : ISingletonService
    { 
        string GetRaw();
        void Update(State update);
    }
}
