using Mairegger.Printing.Content;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Print;

public class PrintContentItem : IPrintContent
{
    public PrintContentItem(UIElement content)
    {
        var container = new Border
        {
            Margin = new Thickness(10),
            Child = content,
            HorizontalAlignment = HorizontalAlignment.Center,
            BorderBrush = new SolidColorBrush(Colors.Black),
            BorderThickness = new Thickness(1),
        };
        this.Content = container;
    }
    public UIElement Content { get; }
}
