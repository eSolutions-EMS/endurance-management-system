using EnduranceJudge.Application;
using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Services;
using System;

namespace Endurance.Judge.Gateways.API.Services
{
    public class StateManager : IStateManager
    {
        private string dataFilePath;

        private readonly IFileService fileService;
        private readonly IJsonSerializationService serializationService;

        public StateManager(IFileService fileService, IJsonSerializationService serializationService)
        {
            this.fileService = fileService;
            this.serializationService = serializationService;
        }

        public void Initialize(string filePath)
        {
            this.dataFilePath = filePath;
        }
        
        public State Get()
        {
            this.Validate();
            
            var contents = this.fileService.Read(this.dataFilePath);
            var state = this.serializationService.Deserialize<State>(contents);
            return state;
        }

        public void Update(State update)
        {
            this.Validate();
            
            var current = this.Get();
            current.MapFrom(update);
            var serialized = this.serializationService.Serialize(current);
            this.fileService.Create(this.dataFilePath, serialized);
        }

        private void Validate()
        {
            if (this.dataFilePath is null)
            {
                throw new Exception("State storage not initialized");
            }
        }
    }

    public interface IStateManager : ISingletonService
    { 
        State Get();
        void Update(State update);
    }
}
