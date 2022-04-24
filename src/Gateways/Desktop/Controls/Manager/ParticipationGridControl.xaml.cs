using EnduranceJudge.Gateways.Desktop.Controls.Manager;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public partial class ParticipationGridControl
{
    public ParticipationGridModel Participation
    {
        get => (ParticipationGridModel)this.GetValue(PARTICIPATION_PROPERTY);
        set => this.SetValue(PARTICIPATION_PROPERTY, value);
    }

    public static readonly DependencyProperty PARTICIPATION_PROPERTY =
        DependencyProperty.Register(
            nameof(Participation),
            typeof(ParticipationGridModel),
            typeof(ParticipationGridControl),
            new PropertyMetadata(OnParticipationChanged));
    private static void OnParticipationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        var grid = (ParticipationGridControl)sender;
        var participation = (ParticipationGridModel) args.NewValue;
        grid.Construct(participation);
    }

    public ParticipationGridControl()
    {
        this.InitializeComponent();
        this.Style = (Style)System.Windows.Application.Current.FindResource("Dock-Horizontal");
    }

    private void Construct(ParticipationGridModel participation)
    {
        this.Performances.Children.Clear();
        foreach (var performance in participation.Performances)
        {
            var control = new PerformanceColumnControl(performance);
            this.Performances.Children.Add(control);
        }
    }
}
