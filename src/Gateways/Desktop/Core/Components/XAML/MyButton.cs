using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Core.Components.XAML;

public class MyButton : Button, IScalableElement
{
    private double? originalWidth;
    private double? originalHeight;
    private double? originalFontSize;

    public MyButton()
    {
        this.FontSize = 15;
    }

    public void ScaleDown(int percent)
    {
        this.originalHeight ??= this.ActualHeight;
        this.originalWidth ??= this.ActualWidth;
        this.originalFontSize ??= this.FontSize;

        var coefficient = percent / 100d;
        this.Height = this.originalHeight.Value * coefficient;
        this.Width = this.originalWidth.Value * coefficient;
        this.FontSize = this.originalFontSize.Value * coefficient;
    }

    public void Restore()
    {
        if (this.originalFontSize is null || this.originalWidth == null || this.originalHeight == null)
        {
            return;
        }
        this.Height = this.originalHeight.Value;
        this.Width = this.originalWidth.Value;
        this.FontSize = this.originalFontSize.Value;
    }
}
