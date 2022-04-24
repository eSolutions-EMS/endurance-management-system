using EnduranceJudge.Gateways.Desktop.Controls.Manager;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public partial class ParticipationControl
{
    public ParticipationControlModel Participation
    {
        get => (ParticipationControlModel)this.GetValue(PARTICIPATION_PROPERTY);
        set => this.SetValue(PARTICIPATION_PROPERTY, value);
    }

    public static readonly DependencyProperty PARTICIPATION_PROPERTY =
        DependencyProperty.Register(
            nameof(Participation),
            typeof(ParticipationControlModel),
            typeof(ParticipationControl),
            new PropertyMetadata(OnParticipationChanged));
    private static void OnParticipationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        var grid = (ParticipationControl)sender;
        var participation = (ParticipationControlModel) args.NewValue;
        grid.Construct(participation);
    }

    public ParticipationControl()
    {
        this.InitializeComponent();
        this.Style = (Style)System.Windows.Application.Current.FindResource("Dock-Horizontal");
    }

    private void Construct(ParticipationControlModel participation)
    {
        this.Performances.Children.Clear();
        foreach (var performance in participation.Performances)
        {
            var control = new PerformanceControl(performance);
            this.Performances.Children.Add(control);
        }
    }
}
