﻿using EnduranceJudge.Core.ConventionalServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnduranceJudge.Gateways.Desktop.Core.Services;

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
        var prev = args.OldFocus;
        if (prev is TextBox prevTextBox)
        {
            prevTextBox.Select(0, 0);
        }
        if (sender is TextBox {IsReadOnly: false} textBox && args.KeyboardDevice.IsKeyDown(Key.Tab))
        {
            textBox.SelectAll();
        }
    }
}

public interface IInputHandler : IService
{
    void HandleScroll(object sender, MouseWheelEventArgs scrollEvent);
}
