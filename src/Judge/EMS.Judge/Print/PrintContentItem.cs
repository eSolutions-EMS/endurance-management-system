using System.Windows;
using System.Windows.Controls;
using Mairegger.Printing.Content;

namespace EMS.Judge.Print;

public class PrintContentItem : IPrintContent
{
    public PrintContentItem(UIElement content)
    {
        var container = new Border
        {
            Margin = new Thickness(10, 0, 10, 0),
            Child = content,
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        this.Content = container;
    }

    public UIElement Content { get; }
}
