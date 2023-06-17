using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace EMS.Judge.Common.Components.Templates.ListItem;

public class ListItemViewModel : BindableBase
{
    private ListItemViewModel()
    {
    }

    public ListItemViewModel(int id, DelegateCommandBase command, string actionName)
        : this(id, id.ToString(), command, actionName)
    {
    }

    public ListItemViewModel(int id, string name, DelegateCommandBase action, string actionName)
        : this(id, name, action, actionName, null)
    {
    }


    public ListItemViewModel(
        int id,
        string name,
        DelegateCommandBase action,
        string actionName,
        DelegateCommandBase remove)
    {
        this.Id = id;
        this.Name = name;
        this.ActionName = actionName;
        this.Action = action;
        this.Remove = remove;
    }

    public int Id { get; }
    public string Name { get; }
    public string ActionName { get; }
    private string _additionalName;
    public string AdditionalName
    {
        get => this._additionalName;
        set => this.SetProperty(ref this._additionalName, value);
    }
    public DelegateCommandBase Action { get; }
    public DelegateCommandBase Remove { get; }
    public Visibility RemoveVisibility
        => this.Remove != null
            ? Visibility.Visible
            : Visibility.Collapsed;
    public DelegateCommandBase AdditionalAction { get; set; }
    public Visibility AdditionalVisibility
        => this.AdditionalAction == null
            ? Visibility.Collapsed
            : Visibility.Visible;
}
