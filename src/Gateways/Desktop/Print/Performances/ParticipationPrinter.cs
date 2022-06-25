using EnduranceJudge.Gateways.Desktop.Controls;
using EnduranceJudge.Gateways.Desktop.Controls.Manager;
using System.Windows;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Print.Performances;

public class ParticipationPrinter : PrintTemplate
{
    private readonly ParticipationGridModel participation;

    public ParticipationPrinter(ParticipationGridModel participation) : base(participation.Number.ToString())
    {
        this.participation = participation;
        var control = new ParticipationGridControl(participation, true);
        // control.Arrange(new Rect());
        // control.Scale(0.75);
        this.AddPrintContent(control);
    }

    public override UIElement GetHeader()
    {
        var header = (StackPanel)base.GetHeader();
        var participantElement = this.CreateParticipantDataElement();

        header.Children.Add(participantElement);
        return header;
    }

    private Border CreateParticipantDataElement()
    {
        var border = new Border
        {
            Style = ControlsHelper.GetStyle("Border"),
            Margin = new Thickness(0, 0, 0, 10),
        };
        var horizontalList = new StackPanel
        {
            Style = ControlsHelper.GetStyle("List-Horizontal"),
        };
        var firstColumn = this.CreateNumberText();
        var secondColumn = this.CreateAthleteData();
        var thirdColumn = this.CreateHorseData();
        var fourthColumn = this.CreateLastColumn();

        horizontalList.Children.Add(firstColumn);
        horizontalList.Children.Add(secondColumn);
        horizontalList.Children.Add(thirdColumn);
        horizontalList.Children.Add(fourthColumn);
        border.Child = horizontalList;

        return border;
    }

    private UIElement CreateNumberText()
        => new TextBlock
        {
            Style = ControlsHelper.GetStyle("Text-Big"),
            Text = this.participation.Participant.Number.ToString(),
            Margin = new Thickness(50, 0, 0, 0),
        };
    private UIElement CreateAthleteData()
        => this.CreateColumn(
            this.participation.Participant.Athlete.Name,
            this.participation.Participant.Athlete.FeiId,
            new Thickness(50, 0, 0, 0));
    private UIElement CreateHorseData()
        => this.CreateColumn(
            this.participation.Participant.Horse.Name,
            this.participation.Participant.Horse.FeiId,
            new Thickness(50, 0, 0, 0));
    private UIElement CreateLastColumn()
        => this.CreateColumn(
            this.participation.Participant.Athlete.Country.Name,
            this.participation.Participant.Athlete.Club,
            new Thickness(50, 0, 0, 0));

    private UIElement CreateColumn(string firstText, string secondText, Thickness margin)
    {

        var column = new StackPanel
        {
            Style = ControlsHelper.GetStyle("List-Vertical"),
            Margin = margin,
        };
        var textStyle = ControlsHelper.GetStyle("Text-Small");
        var nameText = new TextBlock
        {
            Style = textStyle,
            Text = firstText
        };
        var feiIdText = new TextBlock
        {
            Style = textStyle,
            Text = secondText,
        };
        column.Children.Add(nameText);
        column.Children.Add(feiIdText);

        return column;
    }
}
