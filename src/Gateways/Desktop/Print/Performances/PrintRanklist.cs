using EnduranceJudge.Gateways.Desktop.Views.Content.Ranking.ParticipantResults;
using Mairegger.Printing.Content;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Print.Performances;

public class PrintRanklist : PrintTemplate
{
    private readonly IEnumerable<PrintContentItem> printContent;

    public PrintRanklist(string competitionName, IEnumerable<ParticipantResultTemplateModel> participations)
        : base(competitionName)
    {
        var controls = participations
            .Select(template => new PrintContentItem(new ContentControl { Content = template }))
            .ToList();
        this.printContent = controls;
    }

    public override UIElement GetTable(out double reserveHeightOf, out Brush borderBrush)
    {
        reserveHeightOf = this.HeaderOffset;
        borderBrush = this.BorderBrush;
        return new Border();
    }

    public override IEnumerable<IPrintContent> ItemCollection()
        => this.printContent;
}
