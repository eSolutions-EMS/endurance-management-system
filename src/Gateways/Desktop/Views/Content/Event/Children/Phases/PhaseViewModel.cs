using EnduranceJudge.Domain.State.Phases;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Localization;
using System;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Phases
{
    public class PhaseViewModel : FormBase<PhaseView>, IPhaseState
    {
        private string isFinalText;
        private int isFinalValue;
        private int lengthInKm;
        private int orderBy;
        private int maxRecoveryTimeInMinutes;
        private int restTimeInMinutes;

        protected override void Load(int id)
        {
            throw new NotImplementedException();
        }
        protected override void SubmitAction()
        {
            throw new NotImplementedException();
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
