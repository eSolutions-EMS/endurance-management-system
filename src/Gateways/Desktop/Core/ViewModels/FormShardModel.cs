using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public class FormShardModel
    {
        private readonly object dependantCollection;
        private readonly ObservableCollection<ListItemViewModel> itemsCollection;
        private readonly MethodInfo addOrUpdateDependantStatic;

        public FormShardModel(
            object dependantCollection,
            ObservableCollection<ListItemViewModel> itemsCollection,
            MethodInfo addOrUpdateDependantStatic,
            Type viewType)
        {
            this.dependantCollection = dependantCollection;
            this.itemsCollection = itemsCollection;
            this.addOrUpdateDependantStatic = addOrUpdateDependantStatic;
            this.ViewType = viewType;
        }

        public Type ViewType { get; }

        public IList GetDependants()
        {
            return this.dependantCollection as IList;
        }

        public void AddOrUpdateDependant(DependantFormBase dependant)
        {
            this.addOrUpdateDependantStatic.Invoke(null, new[] { this.dependantCollection, dependant });
        }

        public void RefreshItems(IEnumerable<ListItemViewModel> items)
        {
            this.itemsCollection.Clear();
            this.itemsCollection.AddRange(items);
        }
    }
}
