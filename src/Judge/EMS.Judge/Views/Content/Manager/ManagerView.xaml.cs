using System.Windows.Controls;
using System.Windows.Input;
using EMS.Judge.Common;
using EMS.Judge.Common.Services;
using EMS.Judge.Events;
using Prism.Events;

namespace EMS.Judge.Views.Content.Manager;

public partial class ManagerView : UserControl, IView
{
    private readonly IInputHandler inputInput;

    public ManagerView(IInputHandler inputInput, IEventAggregator eventAggregator)
        : this()
    {
        this.inputInput = inputInput;
        eventAggregator
            .GetEvent<SelectTabEvent>()
            .Subscribe(item =>
            {
                ((TabControl)this.FindName("Participations")!).SelectedItem = item;
            });
    }

    public ManagerView()
    {
        InitializeComponent();
    }

    public string RegionName => Regions.CONTENT_LEFT;

    public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
    {
        this.inputInput.HandleScroll(sender, mouseEvent);
    }
}
