using EnduranceJudge.Gateways.Desktop.Core.Components.XAML;
using Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common.Performances;

public class PerformanceColumn : ScalableStackPanel
{
    private PerformanceTemplateModel performance;

    public PerformanceTemplateModel Performance
    {
        get => (PerformanceTemplateModel)GetValue(PERFORMANCE_PROPERTY);
        set => SetValue(PERFORMANCE_PROPERTY, value);
    }

    public static readonly DependencyProperty PERFORMANCE_PROPERTY =
        DependencyProperty.Register(
            nameof(Performance),
            typeof(PerformanceTemplateModel),
            typeof(PerformanceColumn),
            new PropertyMetadata(null, OnPerformanceChanged));

    private static void OnPerformanceChanged(object sender, DependencyPropertyChangedEventArgs args)
    {
        var column = (PerformanceColumn)sender;
        var performance = (PerformanceTemplateModel) args.NewValue;

        column.Construct(performance);
    }

    private void Construct(PerformanceTemplateModel performance)
    {
        this.performance = performance;
        var isEditable = performance.EditVisibility == Visibility.Visible;

        this.AddHeader();
        this.AddArrival(isEditable);
        this.AddInspection(isEditable);
        this.AddReInspection(isEditable);
        this.AddRequiredInspection(isEditable);
        this.AddCompulsoryInspection(isEditable);
        this.AddNextStartTime();
        this.AddRecovery();
        this.AddTime();
        this.AddAverageSpeed();
        this.AddAverageSpeedTotal();
        if (isEditable)
        {
            this.AddEdit();
        }
    }

    private void AddHeader()
        => this.CreateText(this.performance.HeaderValue);
    private void AddArrival(bool isEditable)
        => this.CreateEditable(nameof(this.performance.ArrivalTimeString), isEditable);
    private void AddInspection(bool isEditable)
        => this.CreateEditable(nameof(this.performance.InspectionTimeString), isEditable);
    private void AddReInspection(bool isEditable)
        => this.CreateEditable(nameof(this.performance.ReInspectionTimeString), isEditable);
    private void AddRequiredInspection(bool isEditable)
        => this.CreateEditable(nameof(this.performance.RequiredInspectionTimeString), isEditable);
    private void AddCompulsoryInspection(bool isEditable)
        => this.CreateEditable(nameof(this.performance.CompulsoryRequiredInspectionTimeString), isEditable);
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
        var style = this.GetResource("Button-Table");
        var button = new Button
        {
            Style = style,
            Content = EDIT,
            Command = new DelegateCommand(this.performance.EditAction),
        };
        var border = this.CreateCell(button);
        this.Children.Add(border);
    }

    private void CreateEditable(string propertyName, bool isEditable)
    {
        if (isEditable)
        {
            this.CreateInput(propertyName);
        }
        else
        {
            this.CreateText(propertyName);
        }
    }

    private ScalableBorder CreateCell(UIElement content)
    {
        var style = this.GetResource("Border-Table-Cell");
        var border = new ScalableBorder
        {
            Style = style,
            Child = content,
        };
        return border;
    }

    private void CreateText(string value)
    {
        var style = this.GetResource("Text");
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
        var style = this.GetResource("TextBox-Table");
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

    private Style GetResource(string key)
        => (Style) System.Windows.Application.Current.FindResource(key);
}
