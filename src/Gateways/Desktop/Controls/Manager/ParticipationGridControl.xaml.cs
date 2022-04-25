using Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public partial class ParticipationGridControl
{
    public ParticipationGridModel Participation
    {
        get => (ParticipationGridModel)this.GetValue(PARTICIPATION_PROPERTY);
        set => this.SetValue(PARTICIPATION_PROPERTY, value);
    }

    public bool IsReadonly { get; set; }

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
    public ParticipationGridControl(ParticipationGridModel participation, bool isReadonly) : this()
    {
        this.IsReadonly = isReadonly;
        this.Construct(participation);
    }

    private void Construct(ParticipationGridModel participation)
    {
        this.Performances.Children.Clear();
        foreach (var performance in participation.Performances)
        {
            var control = new PerformanceColumnControl(performance, this.IsReadonly);
            this.Performances.Children.Add(control);
        }
        if (!this.IsReadonly)
        {
            this.AddPrintButton();
        }
    }

    private void AddPrintButton()
    {
        var style = ControlsHelper.GetStyle("Button-Horizontal");
        var button = new Button
        {
            Style = style,
            Content = PRINT,
            Command = new DelegateCommand(this.Participation.PrintAction),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
        };
        this.Children.Add(button);
    }
}
