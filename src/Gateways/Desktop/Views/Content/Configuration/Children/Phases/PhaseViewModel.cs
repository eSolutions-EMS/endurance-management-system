using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.Aggregates.Configuration;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Phases;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Core;
using EnduranceJudge.Localization;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Phases
{
    public class PhaseViewModel : RelatedConfigurationBase<PhaseView, Phase>, IPhaseState
    {
        private readonly IDomainHandler<ConfigurationManager> domainHandler;
        private string isFinalText;
        private int isFinalValue;
        private int lengthInKm;
        private int orderBy;
        private int maxRecoveryTimeInMinutes;
        private int restTimeInMinutes;

        private PhaseViewModel() : this(null, null) { }
        public PhaseViewModel(
            IDomainHandler<ConfigurationManager> domainHandler,
            IQueries<Phase> phases) : base(phases)
        {
            this.domainHandler = domainHandler;
        }

        protected override IDomainObject ActOnSubmit()
        {
            var result = this.domainHandler.Write(manager =>
            {
                if (this.ParentId.HasValue)
                {
                    return manager.Phases.Create(this.ParentId.Value, this);
                }
                else
                {
                    return manager.Phases.Update(this);
                }
            });
            return result;
        }

        public int IsFinalValue
        {
            get => this.isFinalValue;
            set
            {
                this.SetProperty(ref this.isFinalValue, value);
                this.IsFinalText = value == 1
                    ? DesktopStrings.IsFinalText
                    : string.Empty;
            }
        }
        public bool IsFinal => this.IsFinalValue == 1;
        public string IsFinalText
        {
            get => this.isFinalText;
            set => this.SetProperty (ref this.isFinalText, value);
        }
        public int LengthInKm
        {
            get => this.lengthInKm;
            set => this.SetProperty(ref this.lengthInKm, value);
        }
        public int OrderBy
        {
            get => this.orderBy;
            set => this.SetProperty(ref this.orderBy, value);
        }
        public int MaxRecoveryTimeInMins
        {
            get => this.maxRecoveryTimeInMinutes;
            set => this.SetProperty(ref this.maxRecoveryTimeInMinutes, value);
        }
        public int RestTimeInMins
        {
            get => this.restTimeInMinutes;
            set => this.SetProperty(ref this.restTimeInMinutes, value);
        }
    }
}
