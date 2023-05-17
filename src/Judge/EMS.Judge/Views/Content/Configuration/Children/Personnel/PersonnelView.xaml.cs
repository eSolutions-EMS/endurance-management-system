using EMS.Judge.Core;
using EMS.Judge.Core.Services;
using System.Net;
using System.Windows.Controls;
using System.Windows.Input;

namespace EMS.Judge.Views.Content.Configuration.Children.Personnel;

public partial class PersonnelView : UserControl, IView
{
    private readonly IInputHandler inputInput;

    public PersonnelView()
    {
        InitializeComponent();
    }

    public PersonnelView(IInputHandler inputInput) : this()
    {
        this.inputInput = inputInput;
    }

    public string RegionName { get; } = Regions.CONTENT_LEFT;
    public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
    {
        this.inputInput.HandleScroll(sender, mouseEvent);
    }
}
