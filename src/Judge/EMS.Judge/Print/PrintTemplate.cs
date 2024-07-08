using Core.Services;
using Core.Utilities;
using Core.Domain.State;
using Mairegger.Printing.Content;
using Mairegger.Printing.Definition;
using Mairegger.Printing.PrintProcessor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xaml;
using Brushes = System.Windows.Media.Brushes;
using Core.Domain.Enums;

namespace EMS.Judge.Print;

public abstract class PrintTemplate : PrintProcessor
{
    private readonly string title;
    private readonly string category;
    private readonly IFileService file;

    protected PrintTemplate(string title, string category)
    {
        this.title = title;
        this.category = category;
        this.file = StaticProvider.GetService<IFileService>();
        this.State = StaticProvider.GetService<IState>();
    }

    protected IState State { get; }
    protected int HeaderOffset => 10;
    protected SolidColorBrush BorderBrush { get; } = Brushes.DimGray;
    protected List<IPrintContent> PrintItems { get; } = new();

    public override UIElement GetHeader()
    {
        var xaml = this.file.Read("Views/Templates/Print/PrintHeader.xaml");
        var control = (StackPanel)XamlServices.Parse(xaml);
        var eventNameBlock = (TextBlock)control.FindName("EventName")!;
        var populatedPlaceBlock = (Run)control.FindName("PopulatedPlace")!;
        var countryBlock = (Run)control.FindName("CountryName")!;
        var titleBlock = (Run)control.FindName("Title")!;
        var categoryBlock = (Run)control.FindName("Category")!;
        var juryBlock = (Run) control.FindName("PresidentGroundJuryName")!;
        var feiTechBlock = (Run) control.FindName("FeiTechDelegateName")!;
        var feiVetBlock = (Run) control.FindName("FeiVetDelegateName")!;
        var presidentVetBlock = (Run) control.FindName("PresidentVetCommitteeName")!;

        eventNameBlock.Text = $"{this.State.Event.Name} - {DateTime.Now.ToString("dd MMM yyyy", CultureInfo.GetCultureInfo("en-GB"))}";
        populatedPlaceBlock.Text = this.State.Event.PopulatedPlace;
        countryBlock.Text = this.State.Event.Country.Name;
        juryBlock.Text = this.State.Event.PresidentGroundJury?.Name;
        feiTechBlock.Text = this.State.Event.FeiTechDelegate?.Name;
        feiVetBlock.Text = this.State.Event.FeiVetDelegate?.Name;
        presidentVetBlock.Text = this.State.Event.PresidentVetCommittee?.Name;

        titleBlock.Text = title;
        categoryBlock.Text = this.category;

        return control;
    }

    public override UIElement GetTable(out double reserveHeightOf, out Brush borderBrush)
    {
        reserveHeightOf = this.HeaderOffset;
        borderBrush = this.BorderBrush;
        return new Border();
    }

    public override IEnumerable<IPrintContent> ItemCollection()
        => this.PrintItems;

    private UIElement PrepareFooter()
    {
        var xaml = this.file.Read("Views/Templates/Print/PrintFooter.xaml");
        var control = (UserControl)XamlServices.Parse(xaml);
        var presidentGjBlock = (TextBlock)control.FindName("PresidentGroundJury")!;
        var chiefStewBlock = (TextBlock)control.FindName("ChiefSteward")!;
        var dateBlock = (TextBlock)control.FindName("Date")!;

        presidentGjBlock.Text = this.State.Event.PresidentGroundJury?.Name;
        chiefStewBlock.Text = this.State.Event.PresidentVetCommittee?.Name; // TODO: add chief steward personnel?
        dateBlock.Text = DateTime.Now.ToString(CultureInfo.InvariantCulture);;

        return control;
    }

    protected void AddPrintContent(UIElement element)
    {
        var content = new PrintContentItem(element);
        this.PrintItems.Add(content);
    }

    protected override void PreparePrint()
    {
        PrintDefinition.SetPrintAttribute(new PrintOnAllPagesAttribute(PrintAppendixes.Header));
    }
}
