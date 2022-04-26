using EnduranceJudge.Gateways.Desktop.Controls.Ranking;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Controls.Ranking;

public partial class ParticipationResultControl
{
    public ParticipationResultControl()
    {
        InitializeComponent();
    }

    public ParticipationResultModel Participation
    {
        get => (ParticipationResultModel)this.GetValue(PARTICIPATION_PROPERTY);
        set => this.SetValue(PARTICIPATION_PROPERTY, value);
    }

    public static readonly DependencyProperty PARTICIPATION_PROPERTY =
        DependencyProperty.Register(
            nameof(Participation),
            typeof(ParticipationResultModel),
            typeof(ParticipationResultControl),
            new PropertyMetadata(OnParticipationChanged));

    private static void OnParticipationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
        var grid = (ParticipationResultControl)sender;
        var participation = (ParticipationResultModel) args.NewValue;
        grid.Populate(participation);
    }

    private void Populate(ParticipationResultModel participation)
    {
        this.ParticipantNumber.Text = participation.Participant.Number.ToString();
        this.AthleteName.Text = participation.Participant.Athlete.Name;
        this.AthleteCountryName.Text = participation.Participant.Athlete.FeiId;
        this.AthleteFeiId.Text = participation.Participant.Athlete.Country.Name;
        this.HorseName.Text = participation.Participant.Horse.Name;
        this.HorseFeiId.Text = participation.Participant.Horse.FeiId;
        this.TrainerFeiId.Text = participation.Participant.Horse.Breed;
        this.TrainerName.Text = participation.Participant.Horse.TrainerFeiId;
        this.HorseGenderString.Text = participation.HorseGenderString;
        this.RankText.Text = participation.Rank.ToString();
        this.TotalTime.Text = participation.TotalTime;
        this.TotalAverageSpeedString.Text = participation.TotalAverageSpeed;
        this.DisqualifiedReason.Text = participation.DisqualifiedReason;
        this.DisqualifiedContainer.Visibility = participation.DisqualifiedVisibility;
        this.ParticipationGrid.Participation = participation;
    }
}
