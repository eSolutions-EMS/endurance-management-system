using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Core.Components.XAML;

public class ScalableStackPanel : StackPanel, IScalableElement
{
    private double? originalWidth;

    public void ScaleDown(int percent)
    {
        this.originalWidth ??= this.ActualWidth;
        var coefficient = percent / 100d;
        this.Width = this.ActualWidth * coefficient;
    }

    public void Restore()
    {
        if (this.originalWidth.HasValue)
        {
            this.Width = this.originalWidth.Value;
        }
    }
}
