using System.Windows;
using Prism.Commands;

namespace EMS.Judge.Common;

public interface ICollapsable
{
    DelegateCommand ToggleVisibility { get; }
    Visibility Visibility { get; }
    bool IsCollapsed => this.Visibility == Visibility.Collapsed;
    string ToggleText { get; }
}
