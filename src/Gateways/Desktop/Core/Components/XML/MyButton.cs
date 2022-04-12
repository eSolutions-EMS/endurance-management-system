using System.Drawing;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Core.Components.XML;

public partial class MyButton : Button
{
    public MyButton()
    {
        this.FontSize = 15;
    }

    // Scaling up doesn't work, because element is always cropped to it's original size.
    // However scaling down and back up to it's original size works well, at least for
    // buttons.
    public void Scale(int percent)
    {
        var coefficient = percent / 100d;
        var currentFontSize = this.FontSize;
        var currentWidth = this.ActualWidth;
        var currentHeight = this.ActualHeight;

        var newFontSize = currentFontSize * coefficient;
        var newHeight = currentHeight * coefficient;
        var newWidth = currentWidth * coefficient;

        this.FontSize = newFontSize;
        this.Height = newHeight;
        this.Width = newWidth;
    }
}
