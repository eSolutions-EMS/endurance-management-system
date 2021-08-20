using EnduranceJudge.Core.Models;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ComboBoxItem;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System.Collections.Generic;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class FormBase : ViewModelBase, IIdentifiable
    {
        protected FormBase(INavigationService navigation)
        {
            this.Navigation = navigation;
            this.BoolItems = ComboBoxItemViewModel.FromBool();
        }

        protected INavigationService Navigation { get; }
        protected abstract ListItemViewModel ToListItem(DelegateCommand command);

        private int id;
        public int Id
        {
            get => this.id;
            set => this.SetProperty(ref this.id, value);
        }

        public List<ComboBoxItemViewModel> BoolItems { get; set; }

        public bool Equals(IIdentifiable identifiable)
        {
            if (this.Id != default &&  identifiable.Id != default)
            {
                return this.Id == identifiable.Id;
            }

            return false;
        }

        public override bool Equals(object other)
        {
            if (this.Equals(other as IIdentifiable))
            {
                return true;
            }

            return base.Equals(other);
        }

        public override bool Equals(IObject other)
        {
            if (this.Equals(other as IIdentifiable))
            {
                return true;
            }

            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + this.Id;
        }
    }
}
