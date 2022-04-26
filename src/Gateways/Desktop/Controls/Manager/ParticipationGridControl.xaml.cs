using Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public partial class ParticipationGridControl
{
    private bool isPrintButtonAdded;

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

    public ParticipationGridControl(ParticipationGridModel participation, bool isReadonly) : this()
    {
        this.IsReadonly = isReadonly;
        this.Construct(participation);
    }
    public ParticipationGridControl()
    {
        this.InitializeComponent();
        this.Style = (Style)System.Windows.Application.Current.FindResource("Dock-Horizontal");
    }

    private UIElementCollection Columns => this.Table.Children;
    private void Construct(ParticipationGridModel participation)
    {
        // First column is static labels and is defined in the .xaml file
        this.Columns.RemoveRange(1, this.Table.Children.Count - 1);
        foreach (var performance in participation.Performances)
        {
            var control = new PerformanceColumnControl(performance, this.IsReadonly);
            this.Table.Children.Add(control);
        }
        for (var i = 0; i < participation.EmptyColumns; i++)
        {
            var empty = new EmptyColumnControl();
            this.Table.Children.Add(empty);
        }
        if (!this.IsReadonly && !this.isPrintButtonAdded)
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
        this.isPrintButtonAdded = true;
    }
}
