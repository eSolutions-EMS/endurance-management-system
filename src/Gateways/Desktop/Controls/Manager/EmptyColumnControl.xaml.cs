using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public partial class EmptyColumnControl
{
    public EmptyColumnControl()
    {
        this.InitializeComponent();
        for (var i = 0; i < 11; i++)
        {
            var cell = CreateCell();
            this.Children.Add(cell);
        }
    }

    private Border CreateCell()
    {
        var style = ControlsHelper.GetStyle("Border-Performance-Cell");
        var border = new Border
        {
            Style = style,
        };
        return border;
    }
}
