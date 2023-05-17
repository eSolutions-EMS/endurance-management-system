using EMS.Judge.Common.Components.Templates.SimpleListItem;
using EMS.Judge.Services;
using EMS.Judge.Views.Content.Configuration.Core;
using EMS.Judge.Application.Core;
using Core.Domain.AggregateRoots.Configuration;
using Core.Domain.Core.Models;
using Core.Domain.Enums;
using Core.Domain.State.Personnels;
using System.Collections.ObjectModel;
using System.Linq;
using Personnelz = Core.Domain.State.Personnels.Personnel;

namespace EMS.Judge.Views.Content.Configuration.Children.Personnel;

public class PersonnelViewModel : NestedConfigurationBase<PersonnelView, Personnelz>,
    IPersonnelState
{
    private readonly IExecutor<ConfigurationRoot> executor;
    private PersonnelViewModel() : this(null, null) {}
    public PersonnelViewModel(
        IExecutor<ConfigurationRoot> executor,
        IQueries<Personnelz> personnel) : base(personnel)
    {
        this.executor = executor;
    }

    public ObservableCollection<SimpleListItemViewModel> RoleItems { get; }
        = new(SimpleListItemViewModel.FromEnum<PersonnelRole>());

    private string name;
    private int roleId;

    protected override IDomain Persist()
    {
        var result = this.executor.Execute(
            config => config.Save(this),
            true);
        return result;
    }

    public string Name
    {
        get => this.name;
        set => this.SetProperty(ref this.name, value);
    }
    public int RoleId
    {
        get => this.roleId;
        set => this.SetProperty(ref this.roleId, value);
    }

    public PersonnelRole Role => (PersonnelRole)this.RoleId;
    public string RoleName => this.RoleItems.First(x => x.Id == this.RoleId).Name;
}
