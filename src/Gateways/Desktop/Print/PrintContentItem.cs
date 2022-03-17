using Mairegger.Printing.Content;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Print;

public class PrintContentItem : IPrintContent
{
    public PrintContentItem(UIElement content)
    {
        this.Content = content;
    }
    public UIElement Content { get; }
}
