using Endurance.Judge.Gateways.API.Requests;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using System;

namespace Endurance.Judge.Gateways.API.Services
{
    public class Judge : IJudge
    {
        private readonly ManagerRoot manager;
        private readonly IStateManager stateManager;
        public Judge(ManagerRoot manager, IStateManager stateManager)
        {
            this.manager = manager;
            this.stateManager = stateManager;
        }
        
        public void Finish(TagRequest request)
        {
            var time = this.GetSnapshotTime(request.Epoch);
            this.manager.RecordArrive(request.Id, time);
            this.stateManager.Persist();
        }
        
        public void EnterVet(TagRequest request)
        {
            var time = this.GetSnapshotTime(request.Epoch);
            this.manager.RecordArrive(request.Id, time);
            this.stateManager.Persist();
        }

        private DateTime GetSnapshotTime(long epoch)
        {
            var offset = DateTimeOffset.FromUnixTimeMilliseconds(epoch);
            return offset.DateTime;
        }
    }

    public interface IJudge : ITransientService
    {
        void Finish(TagRequest request);
        void EnterVet(TagRequest request);
    }
}
