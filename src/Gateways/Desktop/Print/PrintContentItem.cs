using EnduranceJudge.Gateways.Desktop.Controls;
using Mairegger.Printing.Content;
using System.Windows;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Print;

public class PrintContentItem : IPrintContent
{
    public PrintContentItem(UIElement content)
    {
        var style = ControlsHelper.GetStyle("Padding");
        var container = new Border
        {
            Style = style,
            Child = content,
        };
        this.Content = container;
    }
    public UIElement Content { get; }
}
