using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public partial class ParticipationGridControl
{
    public ParticipationGridModel Participation
    {
        get => (ParticipationGridModel)this.GetValue(PARTICIPATION_PROPERTY);
        set => this.SetValue(PARTICIPATION_PROPERTY, value);
    }

    public bool IsReadonly
    {
        get => (bool)this.GetValue(IS_READONLY_PROPERTY);
        set => this.SetValue(IS_READONLY_PROPERTY, value);
    }

    public static readonly DependencyProperty IS_READONLY_PROPERTY =
        DependencyProperty.Register(
            nameof(IsReadonly),
            typeof(bool),
            typeof(ParticipationGridControl));

    public static readonly DependencyProperty PARTICIPATION_PROPERTY =
        DependencyProperty.Register(
            nameof(Participation),
            typeof(ParticipationGridModel),
            typeof(ParticipationGridControl),
            new PropertyMetadata(OnParticipationChanged));

    private static void OnParticipationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        var grid = (ParticipationGridControl)sender;
        if (args.NewValue is not null)
        {
            var participation = (ParticipationGridModel) args.NewValue;
            grid.Table.ItemsSource = participation.Performances;
        }
    }

    public ParticipationGridControl()
    {
        this.InitializeComponent();
        this.Style = (Style)System.Windows.Application.Current.FindResource("Dock-Horizontal");
    }
}
