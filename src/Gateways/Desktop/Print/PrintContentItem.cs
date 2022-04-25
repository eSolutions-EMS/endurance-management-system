using Mairegger.Printing.Content;
using System.Windows;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Print;

public class PrintContentItem : IPrintContent
{
    public PrintContentItem(UIElement content)
    {
        var container = new Border
        {
            Padding = new Thickness(10),
            Child = content,
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        this.Content = container;
    }
    public UIElement Content { get; }
}
