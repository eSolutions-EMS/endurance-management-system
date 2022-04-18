using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Core.Components.XAML;

public class ScalableTextBlock : TextBlock, IScalableElement
{
    private double? originalFontSize;

    public void ScaleDown(int percent)
    {
        this.originalFontSize ??= this.FontSize;
        var coefficient = percent / 100d;
        this.FontSize *= coefficient;
    }

    public void Restore()
    {
        if (this.originalFontSize.HasValue)
        {
            this.FontSize = this.originalFontSize.Value;
        }
    }
}
