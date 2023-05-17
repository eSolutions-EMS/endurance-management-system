using EMS.Judge.Core.Components.Templates.ListItem;
using EMS.Judge.Core.Services;
using EMS.Judge.Services;
using EMS.Judge.Application.Services;
using EMS.Judge.Application.Core.Models;
using Core.Utilities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Core.Localization.Strings;

namespace EMS.Judge.Core.ViewModels;

public abstract class ListViewModelBase<TView> : ViewModelBase
    where TView : IView
{
    private readonly IPersistence persistence;
    private readonly IPopupService popupService;
    protected ListViewModelBase(INavigationService navigation, IPersistence persistence, IPopupService popupService)
    {
        this.Executor = StaticProvider.GetService<IExecutor>();
        this.persistence = persistence;
        this.popupService = popupService;
        this.Navigation = navigation;
        this.Create = new DelegateCommand(this.CreateAction);
    }

    protected bool AllowDelete { get; init; } = true;
    protected bool AllowCreate { get; init; } = true;
    protected INavigationService Navigation { get; }
    protected IExecutor Executor { get; }

    public ObservableCollection<ListItemViewModel> ListItems { get; protected init; }
        = new (Enumerable.Empty<ListItemViewModel>());

    public DelegateCommand Create { get; }

    public override void OnNavigatedTo(Prism.Regions.NavigationContext context)
    {
        base.OnNavigatedTo(context);
        this.Load();
    }

    protected abstract IEnumerable<ListItemModel> LoadData();
    protected virtual void RemoveDomain(int id)
    {
        throw new NotImplementedException();
    }

    protected virtual void Load() => this.Executor.Execute(() =>
    {
        var eventsList = this.LoadData();

        var viewModels = eventsList
            .Select(this.ToViewModel)
            .ToList();

        this.ListItems.Clear();
        this.ListItems.AddRange(viewModels);
    }, false);
    
    protected virtual void CreateAction()
    {
        if (this.AllowCreate)
        {
            this.Navigation.ChangeTo<TView>();
        }
    }
    protected virtual void UpdateAction(int? id)
    {
        this.Navigation.ChangeToUpdateConfiguration<TView>(id!.Value);
    }
    protected virtual void RemoveAction(int? id)
    {
        Action action = () => this.Executor.Execute(() =>
        {
            if (this.AllowDelete)
            {
                this.RemoveDomain(id!.Value);
                var item = this.ListItems.FirstOrDefault(i => i.Id == id!.Value);
                this.ListItems.Remove(item);
            }
        }, true);
        this.popupService.RenderConfirmation(REMOVE_CONFIRMATION_MESSAGE, action);
    }

    protected virtual ListItemViewModel ToViewModel(ListItemModel listable)
    {
        var update = new DelegateCommand<int?>(this.UpdateAction);
        var remove = this.AllowDelete
            ? new DelegateCommand<int?>(this.RemoveAction)
            : null;
        return new ListItemViewModel(listable.Id, listable.Name, update, UPDATE, remove);
    }
}
