using EnduranceJudge.Gateways.Desktop.Core.Components.XAML;
using Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public class PerformanceColumnControl : ScalableStackPanel
{
    private PerformanceColumnModel performance;

    public PerformanceColumnModel Performance
    {
        get => (PerformanceColumnModel)GetValue(PERFORMANCE_PROPERTY);
        set => SetValue(PERFORMANCE_PROPERTY, value);
    }
    public bool IsReadonly { get; set; }

    public static readonly DependencyProperty PERFORMANCE_PROPERTY =
        DependencyProperty.Register(
            nameof(Performance),
            typeof(PerformanceColumnModel),
            typeof(PerformanceColumnControl),
            new PropertyMetadata(null, OnPerformanceChanged));

    private static void OnPerformanceChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
        var column = (PerformanceColumnControl)sender;
        var performance = (PerformanceColumnModel) args.NewValue;
        column.Construct(performance);
    }

    public PerformanceColumnControl() {}
    public PerformanceColumnControl(PerformanceColumnModel performance, bool isReadonly)
    {
        this.IsReadonly = isReadonly;
        this.Construct(performance);
    }

    private void Construct(PerformanceColumnModel performance)
    {
        this.performance = performance;

        this.AddHeader();
        if (this.IsReadonly)
        {
            this.AddArrivalText();
            this.AddInspectionText();
            this.AddReInspectionText();
            this.AddRequiredInspectionText();
            this.AddCompulsoryInspectionText();
        }
        else
        {
            this.AddArrivalInput();
            this.AddInspectionInput();
            this.AddReInspectionInput();
            this.AddRequiredInspectionInput();
            this.AddCompulsoryInspectionInput();
        }
        this.AddNextStartTime();
        this.AddRecovery();
        this.AddTime();
        this.AddAverageSpeed();
        this.AddAverageSpeedTotal();
        if (!this.IsReadonly)
        {
            this.AddEdit();
        }
    }

    private void AddHeader()
        => this.CreateText(this.performance.HeaderValue);
    private void AddArrivalInput()
        => this.CreateInput(nameof(this.performance.ArrivalTimeString));
    private void AddInspectionInput()
        => this.CreateInput(nameof(this.performance.InspectionTimeString));
    private void AddReInspectionInput()
        => this.CreateInput(nameof(this.performance.ReInspectionTimeString));
    private void AddRequiredInspectionInput()
        => this.CreateInput(nameof(this.performance.RequiredInspectionTimeString));
    private void AddCompulsoryInspectionInput()
        => this.CreateInput(nameof(this.performance.CompulsoryRequiredInspectionTimeString));
    private void AddArrivalText()
        => this.CreateText(this.performance.ArrivalTimeString);
    private void AddInspectionText()
        => this.CreateText(this.performance.InspectionTimeString);
    private void AddReInspectionText()
        => this.CreateText(this.performance.ReInspectionTimeString);
    private void AddRequiredInspectionText()
        => this.CreateText(this.performance.RequiredInspectionTimeString);
    private void AddCompulsoryInspectionText()
        => this.CreateText(this.performance.CompulsoryRequiredInspectionTimeString);
    private void AddNextStartTime()
        => this.CreateText(this.performance.NextStartTimeString);
    private void AddRecovery()
        => this.CreateText(this.performance.RecoverySpanString);
    private void AddTime()
        => this.CreateText(this.performance.TimeString);
    private void AddAverageSpeed()
        => this.CreateText(this.performance.AverageSpeedString);
    private void AddAverageSpeedTotal()
        => this.CreateText(this.performance.AverageSpeedTotalString);
    private void AddEdit()
    {
        var style = ControlsHelper.GetStyle("Button-Table");
        var button = new Button
        {
            Style = style,
            Content = EDIT,
            Command = new DelegateCommand(this.performance.EditAction),
        };
        var border = this.CreateCell(button);
        this.Children.Add(border);
    }

    private ScalableBorder CreateCell(UIElement content)
    {
        var style = ControlsHelper.GetStyle("Border-Table-Cell");
        var border = new ScalableBorder
        {
            Style = style,
            Child = content,
        };
        return border;
    }

    private void CreateText(string value)
    {
        var style = ControlsHelper.GetStyle("Text");
        var text = new ScalableTextBlock
        {
            Text = value,
            Style = style,
        };
        var border = this.CreateCell(text);
        this.Children.Add(border);
    }

    private void CreateInput(string propertyName)
    {
        var style = ControlsHelper.GetStyle("TextBox-Table");
        var input = new TextBox
        {
            Style = style,
            DataContext = this.performance,
            BorderThickness = new Thickness(0),
        };
        input.SetBinding(TextBox.TextProperty, new Binding(propertyName));
        var border = this.CreateCell(input);
        this.Children.Add(border);
    }
}
