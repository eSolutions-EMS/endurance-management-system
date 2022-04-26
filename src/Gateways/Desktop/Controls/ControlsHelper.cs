using System.Windows;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Controls;

public static class ControlsHelper
{
    public static Style GetStyle(string key)
        => (Style) System.Windows.Application.Current.FindResource(key);

    public static void Scale(this UIElement control, double coefficient)
        => ScaleElementTree(control, coefficient);

    /// <summary>
    /// Limited functionality.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="coefficient"></param>
    private static void ScaleElementTree(UIElement element, double coefficient)
    {
        ScaleElement(element, coefficient);
        if (element is Decorator decorator)
        {
            ScaleElementTree(decorator.Child, coefficient);
        }
        else if (element is Panel panel)
        {
            foreach (var child in panel.Children)
            {
                if (child is UIElement uiElement)
                {
                    ScaleElementTree(uiElement, coefficient);
                }
            }
        }
    }

    private static void ScaleElement(UIElement element, double coefficient)
    {
        if (element is Border border)
        {
            var padding = new Thickness(
                border.Padding.Left * coefficient,
                border.Padding.Top * coefficient,
                border.Padding.Right * coefficient,
                border.Padding.Bottom * coefficient);
            border.Padding = padding;
            border.Height = border.ActualHeight * coefficient;
            border.Width = border.ActualWidth * coefficient;
            // if (border.Name == "RootBorder")
            // {
            //     border.Width = 1000;
            // }
        }
        else if (element is TextBlock textBlock)
        {
            textBlock.FontSize *= coefficient;
        }
    }
}
