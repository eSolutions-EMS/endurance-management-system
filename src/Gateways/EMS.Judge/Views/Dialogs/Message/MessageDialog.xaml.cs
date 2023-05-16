using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using System.Windows;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Views.Dialogs.Message;

public partial class MessageDialog : IScrollableVisual
{
    private readonly IInputHandler inputHandler;
    public MessageDialog(IInputHandler inputHandler)
    {
        this.inputHandler = inputHandler;
        this.InitializeComponent();
        this.MaxWidth = SystemParameters.PrimaryScreenWidth - 20;
        this.MaxHeight = SystemParameters.PrimaryScreenHeight - 20;
    }

    public void HandleScroll(object sender, MouseWheelEventArgs args)
        => this.inputHandler.HandleScroll(sender, args);
}
