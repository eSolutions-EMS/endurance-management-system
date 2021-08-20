using EnduranceJudge.Application.Core.Exceptions;
using EnduranceJudge.Core;
using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Gateways.Desktop.Core.Components.Templates.ListItem;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace EnduranceJudge.Gateways.Desktop.Core.ViewModels
{
    public abstract class ShardableFormBase : FormBase
    {
        private readonly Dictionary<string, FormShardModel> shards = new();
        private readonly Type thisType;

        protected ShardableFormBase(INavigationService navigation) : base(navigation)
        {
            this.thisType = this.GetType();

            if (this is IPrincipalForm)
            {
                this.InitializeShards();
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            foreach (var (key, shard) in this.shards)
            {
                this.UpdateListItems(key, shard);
            }
        }

        private Action<object> GetDependantSubmitAction(string key)
        {
            Action<object> action = obj =>
            {
                if (obj is not DependantFormBase dependant)
                {
                    throw new AppException("kur");
                }

                this.UpdateDependants(key, dependant);
            };

            return action;
        }

        private void UpdateDependants(string key, DependantFormBase dependant)
        {
            if (!this.shards.ContainsKey(key))
            {
                throw new Exception($"Cannot add dependant - key '{key}' does not exist in dependants dictionary.");
            }

            var shard = this.shards[key];

            shard.AddOrUpdateDependant(dependant);
            this.UpdateListItems(key, shard);
        }

        private void UpdateListItems(string key, FormShardModel shard)
        {
            var listItems = new List<ListItemViewModel>();
            foreach (var item in shard.GetDependants())
            {
                if (item is not DependantFormBase dependant)
                {
                    throw new Exception("kur");
                }

                var listItem = this.ConvertToListItem(key, dependant, shard.ViewType);
                listItems.Add(listItem);
            }

            shard.RefreshItems(listItems);
        }

        private ListItemViewModel ConvertToListItem(string key, DependantFormBase dependant, Type view)
        {
            var submitAction = this.GetDependantSubmitAction(key);

            var navigateToUpdateAction = this.NavigateToDependantUpdateDelegate(view, dependant, submitAction);
            var command = new DelegateCommand(navigateToUpdateAction);

            var listItem = dependant.ToListItem(command);
            return listItem;
        }

        private void InitializeShards()
        {
            var principalFormType = DesktopConstants.Types.PrincipalForm;

            var shards = this.thisType
                .GetInterfaces()
                .Where(type => principalFormType.IsAssignableFrom(type) && principalFormType != type)
                .ToList();

            var thisProperties = ReflectionUtilities.GetProperties(this.thisType);

            foreach (var shard in shards)
            {
                this.InitializeShard(shard, thisProperties);
            }
        }

        private void InitializeShard(Type hasDependantsInterface, PropertyInfo[] thisProperties)
        {
            var interfaceProperties = ReflectionUtilities.GetProperties(hasDependantsInterface);

            var dependantType = interfaceProperties
                .Single(info => info.PropertyType.Name == CoreConstants.Types.ListGeneric.Name)
                .PropertyType
                .GetGenericArguments()
                .Single();

            var dependantViewType = ReflectionUtilities
                .GetGenericArguments(hasDependantsInterface)
                .First();

            var dependantKey = dependantType.FullName;

            var shard = this.CreatePrincipalShardModel(dependantType, dependantViewType, interfaceProperties);
            this.shards.Add(dependantKey!, shard);

            this.SetNavigateToCreateCommand(dependantKey, dependantViewType, interfaceProperties, thisProperties);
        }

        private FormShardModel CreatePrincipalShardModel(
            Type dependantType,
            Type dependantViewType,
            PropertyInfo[] interfaceProperties)
        {
            var dependantCollectionType = CoreConstants.Types.ListGeneric.MakeGenericType(dependantType);
            var itemsCollectionType = DesktopConstants.Types.ObservableListItems;

            var dependantsCollectionInfo = interfaceProperties.First(t =>
                dependantCollectionType.IsAssignableFrom(t.PropertyType));

            var itemsCollectionInfo = interfaceProperties.First(t =>
                itemsCollectionType.IsAssignableFrom(t.PropertyType));

            // Dependants.AddOrUpdate(...)
            var addOrUpdateObjectStaticMethod = CoreConstants.Types.ObjectExtensions
                .GetMethod(nameof(IObjectExtensions.AddOrUpdateObject))
                !.MakeGenericMethod(dependantType);

            var dependantsCollection =  dependantsCollectionInfo.GetValue(this);
            var itemsCollection = itemsCollectionInfo.GetValue(this) as ObservableCollection<ListItemViewModel>;

            var principalShard = new FormShardModel(
                dependantsCollection,
                itemsCollection,
                addOrUpdateObjectStaticMethod,
                dependantViewType);

            return principalShard;
        }

        private void SetNavigateToCreateCommand(
            string key,
            Type dependantViewType,
            PropertyInfo[] interfaceProperties,
            PropertyInfo[] thisProperties)
        {
            var submitAction = this.GetDependantSubmitAction(key);
            var navigateAction = this.NavigateToDependantCreateDelegate(dependantViewType, submitAction);
            var navigateCommand = new DelegateCommand(navigateAction);

            var navigateCommandName = interfaceProperties
                .First(t => DesktopConstants.Types.DelegateCommand.IsAssignableFrom(t.PropertyType))
                !.Name;

            var navigateCommandInfo = thisProperties.First(t => t.Name == navigateCommandName);

            var setter = navigateCommandInfo!.GetSetMethod(nonPublic: true);
            if (setter == null)
            {
                throw new Exception($"Add private setter to {navigateCommandInfo.Name}");
            }

            navigateCommandInfo.SetValue(this, navigateCommand);
        }

        private Action NavigateToDependantCreateDelegate(Type viewType, Action<object> action)
        {
            return () => this.Navigation.ChangeTo(viewType, action);
        }

        private Action NavigateToDependantUpdateDelegate(Type viewType, object data, Action<object> action)
        {
            return () => this.Navigation.ChangeTo(viewType, data, action);
        }
    }
}
