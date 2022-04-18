using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Core.Components.XAML;

public class ScalableBorder : Border, IScalableElement
{
    private double? originalHeight;

    public void ScaleDown(int percent)
    {
        this.originalHeight ??= this.ActualHeight;
        var coefficient = percent / 100d;
        this.Height = this.ActualHeight * coefficient;
    }
    public void Restore()
    {
        if (this.originalHeight.HasValue)
        {
            this.Height = this.originalHeight.Value;
        }
    }
}
