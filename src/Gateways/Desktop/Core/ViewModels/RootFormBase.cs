using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using EnduranceJudge.Gateways.Desktop.Core.Extensions;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using MediatR;
using Prism.Commands;
using Prism.Regions;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class RootFormBase<TQuery, TCommand, TUpdateModel, TView> : ParentFormBase<TView>,
        IMapTo<TCommand>,
        IMapFrom<TUpdateModel>
        where TCommand : IRequest
        where TQuery : IRequest<TUpdateModel>, IIdentifiable, new()
        where TView : IView
    {
        protected RootFormBase(IApplicationService application)
        {
            this.Application = application;
            this.Submit = new AsyncCommand(this.SubmitAction);
        }

        private bool IsCreateForm => this.Id == default;

        protected IApplicationService Application { get; }

        public DelegateCommand Submit { get; }
        protected virtual async Task SubmitAction()
        {
            var command = this.Map<TCommand>();
            await this.Application.Execute(command);
        }

        public override bool IsNavigationTarget(NavigationContext context)
        {
            if (this.IsCreateForm)
            {
                return true;
            }

            var id = context.GetId();
            return this.Id == id;
        }

        public override void OnNavigatedTo(NavigationContext context)
        {
            var id = context.GetId();
            if (id.HasValue && this.Id == default)
            {
                this.Load(id.Value);
            }
            base.OnNavigatedTo(context);
        }

        private async Task Load(int id)
        {
            if (this.Id != default)
            {
                return;
            }

            var command = new TQuery
            {
                Id = id,
            };
            var enduranceEvent = await this.Application.Execute(command);
            this.MapFrom(enduranceEvent);
        }
    }
}
