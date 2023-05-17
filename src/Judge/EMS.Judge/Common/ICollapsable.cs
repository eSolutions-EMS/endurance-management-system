using Prism.Commands;
using System.Windows;

namespace EMS.Judge.Common;

public interface ICollapsable
{
    DelegateCommand ToggleVisibility { get; }
    Visibility Visibility { get; }
    bool IsCollapsed => this.Visibility == Visibility.Collapsed;
    string ToggleText { get; }
}
