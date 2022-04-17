namespace EnduranceJudge.Gateways.Desktop.Core.Components.XAML;

// Scaling up doesn't work, because element is always cropped to it's original size.
// However scaling down and back up to it's original size works well, at least for
// buttons.
public interface IScalableElement
{
    void ScaleDown(int percent);
    void Restore();
}
