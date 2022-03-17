using EnduranceJudge.Core.Services;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.State;
using Mairegger.Printing.Definition;
using Mairegger.Printing.PrintProcessor;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xaml;

namespace EnduranceJudge.Gateways.Desktop.Print;

public abstract class PrintTemplate : PrintProcessor
{
    private readonly string title;
    private readonly IFileService file;
    private readonly UIElement header;
    private readonly UIElement footer;

    protected PrintTemplate(string title)
    {
        this.title = title;
        this.file = StaticProvider.GetService<IFileService>();
        this.State = StaticProvider.GetService<IState>();

        this.header = this.PrepareHeader();
        this.footer = this.PrepareFooter();
    }

    protected IState State { get; }
    protected int HeaderOffset { get; set; } = 0;
    protected SolidColorBrush BorderBrush { get; set; } = Brushes.DimGray;

    public override UIElement GetHeader()
        => this.header;

    public override UIElement GetFooter()
        => this.footer;
    private UIElement PrepareHeader()
    {
        var xaml = this.file.Read("Views/Templates/Print/Header.xaml");
        var control = (UserControl)XamlServices.Parse(xaml);
        var eventNameBlock = (TextBlock)control.FindName("EventName")!;
        var populatedPlaceBlock = (Run)control.FindName("PopulatedPlace")!;
        var countryBlock = (Run)control.FindName("CountryName")!;
        var dateBlock = (TextBlock)control.FindName("Date")!;
        var organizerBlock = (TextBlock)control.FindName("Organizer")!;
        var titleBlock = (TextBlock)control.FindName("Title")!;

        eventNameBlock.Text = this.State.Event.Name;
        populatedPlaceBlock.Text = this.State.Event.PopulatedPlace;
        countryBlock.Text = this.State.Event.Country.Name;
        dateBlock.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture); // TODO fix date
        organizerBlock.Text = "BFKS"; // TODO attach on Event
        titleBlock.Text = title;

        return control;
    }

    private UIElement PrepareFooter()
    {
        var xaml = this.file.Read("Views/Templates/Print/Footer.xaml");
        var control = (UserControl)XamlServices.Parse(xaml);
        var presidentGjBlock = (TextBlock)control.FindName("PresidentGroundJury")!;
        var chiefStewBlock = (TextBlock)control.FindName("ChiefSteward")!;
        var dateBlock = (TextBlock)control.FindName("Date")!;

        presidentGjBlock.Text = this.State.Event.PresidentGroundJury?.Name;
        chiefStewBlock.Text = this.State.Event.PresidentVetCommission?.Name; // TODO: add chief steward personnel?
        dateBlock.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);;

        return control;
    }

    protected override void PreparePrint()
    {
        PrintDefinition.SetPrintAttribute(new PrintOnAllPagesAttribute(PrintAppendixes.Footer));
        PrintDefinition.SetPrintAttribute(new PrintOnAllPagesAttribute(PrintAppendixes.Header));
    }
}
