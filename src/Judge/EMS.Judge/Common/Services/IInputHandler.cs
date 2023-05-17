using Core.ConventionalServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EMS.Judge.Common.Services;

public class InputHandler : IInputHandler
{
    public InputHandler()
    {
        EventManager.RegisterClassHandler(
            typeof(TextBox),
            UIElement.GotKeyboardFocusEvent,
            new KeyboardFocusChangedEventHandler(this.HandleKeyboardFocus));
    }

    public void HandleScroll(object sender, MouseWheelEventArgs scrollEvent)
    {
        var scv = (ScrollViewer)sender;
        scv.ScrollToVerticalOffset(scv.VerticalOffset - scrollEvent.Delta);
        scrollEvent.Handled = true;
    }

    private void HandleKeyboardFocus(object sender, KeyboardFocusChangedEventArgs args)
    {
        if (sender is TextBox {IsReadOnly: false} textBox && args.KeyboardDevice.IsKeyDown(Key.Tab))
        {
            textBox.SelectAll();
        }
    }
}

public interface IInputHandler : ITransientService
{
    void HandleScroll(object sender, MouseWheelEventArgs scrollEvent);
}
