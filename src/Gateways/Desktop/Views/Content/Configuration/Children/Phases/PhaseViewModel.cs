﻿using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Domain.AggregateRoots.Configuration;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Phases;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Core;
using EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Phases
{
    public class PhaseViewModel : NestedConfigurationBase<PhaseView, Phase>, IPhaseState
    {
        private readonly ConfigurationRoot aggregate;
        private string isFinalText;
        private int isFinalValue;
        private double? lengthInKm;
        private int? orderBy;
        private int? maxRecoveryTimeInMinutes;
        private int? restTimeInMinutes;
        private bool requireCompulsoryInspection;

        private PhaseViewModel() : this(null, null) { }
        public PhaseViewModel(
            ConfigurationRoot aggregate,
            IQueries<Phase> phases) : base(phases)
        {
            this.aggregate = aggregate;
        }

        protected override IDomain Persist()
        {
            if (this.ParentId.HasValue)
            {
                return this.aggregate.Phases.Create(this.ParentId.Value, this);
            }
            else
            {
                return this.aggregate.Phases.Update(this);
            }
        }

        public int IsFinalValue
        {
            get => this.isFinalValue;
            set
            {
                this.SetProperty(ref this.isFinalValue, value);
                this.IsFinalText = value == 1
                    ? Words.FINAL
                    : string.Empty;
            }
        }
        public bool IsFinal => this.IsFinalValue == 1;
        public string IsFinalText
        {
            get => this.isFinalText;
            set => this.SetProperty (ref this.isFinalText, value);
        }
        public double? LengthInKmDisplay
        {
            get => this.lengthInKm;
            set => this.SetProperty(ref this.lengthInKm, value);
        }
        public int? OrderByDisplay
        {
            get => this.orderBy;
            set => this.SetProperty(ref this.orderBy, value);
        }
        public int? MaxRecoveryTimeInMinsDisplay
        {
            get => this.maxRecoveryTimeInMinutes;
            set => this.SetProperty(ref this.maxRecoveryTimeInMinutes, value);
        }
        public int? RestTimeInMinsDisplay
        {
            get => this.restTimeInMinutes;
            set => this.SetProperty(ref this.restTimeInMinutes, value);
        }

        public double LengthInKm
        {
            get => this.LengthInKmDisplay ?? default;
            set => this.LengthInKmDisplay = value;
        }
        public int OrderBy
        {
            get => this.OrderByDisplay ?? default;
            set => this.OrderByDisplay = value;
        }
        public int MaxRecoveryTimeInMins
        {
            get => this.MaxRecoveryTimeInMinsDisplay ?? default;
            set => this.MaxRecoveryTimeInMinsDisplay = value;
        }
        public int RestTimeInMins
        {
            get => this.RestTimeInMinsDisplay ?? default;
            set => this.RestTimeInMinsDisplay = value;
        }
        public bool IsCompulsoryInspectionRequired
        {
            get => this.requireCompulsoryInspection;
            set => this.SetProperty(ref this.requireCompulsoryInspection, value);
        }
    }
}
