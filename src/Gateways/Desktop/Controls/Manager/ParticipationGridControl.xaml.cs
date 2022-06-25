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
        if (args.NewValue is not null)
        {
            var participation = (ParticipationGridModel) args.NewValue;
            grid.Populate(participation);
        }
    }

    public ParticipationGridControl(ParticipationGridModel participation, bool isReadonly) : this()
    {
        this.IsReadonly = isReadonly;
        this.Participation = participation;
    }
    public ParticipationGridControl()
    {
        this.InitializeComponent();
        this.Style = (Style)System.Windows.Application.Current.FindResource("Dock-Horizontal");
    }

    private void Populate(ParticipationGridModel participation)
    {
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
