using EMS.Judge.Common;
using EMS.Judge.Common.Services;
using EMS.Judge.Views.Content.Hardware.Tags;
using EMS.Judge.Views.Content.Manager;
using EMS.Judge.Application.Common;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.State.Participations;
using EMS.Judge.Application.Hardware;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Media;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EMS.Judge.Views.Content.Hardware;

public class HardwareViewModel : ViewModelBase
{
    private readonly IQueries<Participation> participationQueries;
    private readonly IPopupService popupService;
    private readonly VupRfidController controller;
    private string message = "";
    private int power = 27;
    private bool isListing;

    public HardwareViewModel(IQueries<Participation> participationQueries, IPopupService popupService)
    {
        this.participationQueries = participationQueries;
        this.popupService = popupService;
        this.controller = new VupRfidController(RfidWitness.FINISH_DEVICE_IP);
        this.controller.MessageEvent += (_, message) => this.Message = message;
        this.controller.ReadEvent += this.HandleReadEventTag;
        this.isListing = this.controller.IsPolling;

        this.Connect = new DelegateCommand(this.ConnectAction);
        this.Start = new DelegateCommand(this.StartAction);
        this.Stop = new DelegateCommand(this.StopAction);
        this.SetPower = new DelegateCommand(this.SetPowerAction);
        this.Reset = new DelegateCommand(this.ResetAction);
        this.Disconnect = new DelegateCommand(this.DisconnectAction);
        this.Stats = new DelegateCommand(this.GenerateRfidStats);
    }

    public ObservableCollection<TagViewModel> Tags { get; } = new();
    public DelegateCommand Connect { get; }
    public DelegateCommand Start { get; }
    public DelegateCommand Stop { get; }
    public DelegateCommand SetPower { get; }
    public DelegateCommand Reset { get; }
    public DelegateCommand Disconnect { get; }
    public DelegateCommand Stats { get; }

    public string Message
    {
        get => this.message;
        set => this.SetProperty(ref this.message, value);
    }
    public string Power
    {
        get => this.power.ToString();
        set => this.SetProperty(ref this.power, int.Parse(value));
    }
    public bool IsListing
    {
        get => this.isListing;
        set
        {
            this.SetProperty(ref this.isListing, value);
            this.RaisePropertyChanged(nameof (this.IsNotListing));
        }
    }
    public bool IsNotListing
    {
        get => !isListing;
    }

    public void ConnectAction()
    {
        this.controller.Connect();
    }

    public void StartAction()
    {
        this.IsListing = true;
        Task.Run(() => this.controller.StartPolling());
    }

    public void StopAction()
    {
        this.IsListing = false;
        this.controller.StopPolling();
    }

    public void SetPowerAction()
    {
        this.controller.SetPower(this.power);
    }

    public void ResetAction()
    {
        foreach (var tag in this.Tags)
        {
            tag.DetectedCount = 0;
        }
    }

    public void DisconnectAction()
    {
        Task.Run(() => this.controller.Disconnect());
    }

    private void HandleReadEventTag(object sender, IEnumerable<string> tagIds)
    {
        ThreadPool.QueueUserWorkItem(delegate
        {
            SynchronizationContext.SetSynchronizationContext(new
                DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher));

            SynchronizationContext.Current!.Post(_ =>
            {
                SystemSounds.Beep.Play();
                foreach (var tagId in tagIds)
                {
                    var existingTag = this.Tags.FirstOrDefault(x => x.Id == tagId);
                    if (existingTag != null)
                    {
                        existingTag.Detect();
                    }
                    else
                    {
                        var tag = new TagViewModel { DetectedCount = 1, Id = tagId };
                        this.Tags.Add(tag);
                    }
                }
            }, null);
        });
    }

    private void GenerateRfidStats()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Participations:");

        var overallArrRates = new List<double>();
        var overallVetRates = new List<double>();
        var overallAverages = new List<double>();
        foreach (var participation in this.participationQueries.GetAll().OrderBy(x => x.Participant.Number))
        {
            var laps = participation.Participant.LapRecords;
            var ARRs = laps.Count;
            var INs = laps.Count
                + laps.Count(x => x.IsReinspectionRequired)
                + laps.Count(x => x.IsRequiredInspectionRequired);

            var neckArrDetections = participation.Participant.DetectedNeck[WitnessEventType.Arrival];
            var headArrDetections = participation.Participant.DetectedHead[WitnessEventType.Arrival];
            var neckVetDetections = participation.Participant.DetectedNeck[WitnessEventType.VetIn];
            var headVetDetections = participation.Participant.DetectedHead[WitnessEventType.VetIn];

            var headArrRate = (double)headArrDetections.Distinct().Count() / ARRs;
            var headVetRate = (double) headVetDetections.Distinct().Count() / INs;
            var neckArrRate = (double)neckArrDetections.Distinct().Count() / ARRs;
            var neckVetRate = (double) neckVetDetections.Distinct().Count() / INs;

            var overallArrRate = (double) headArrDetections.Concat(neckArrDetections).Distinct().Count() / ARRs;
            var overallVetRate = (double) headVetDetections.Concat(neckVetDetections).Distinct().Count() / INs;
            var overallAverage = (overallArrRate + overallVetRate) / 2;
            overallArrRates.Add(overallArrRate);
            overallVetRates.Add(overallVetRate);
            overallAverages.Add(overallAverage);

            sb.AppendLine($"# {participation.Participant.Number} #".PadRight(75, '#'));
            sb.AppendLine($"head - {participation.Participant.RfIdHead,24} " +
                $" - arr: {headArrDetections.Count}/{ARRs} ({this.FormatRate(headArrRate)})" +
                $" - vet: {headVetDetections.Count}/{INs} ({this.FormatRate(headVetRate)})");
            sb.AppendLine($"neck - {participation.Participant.RfIdNeck,24} " +
                $" - arr: {neckArrDetections.Count}/{ARRs} ({this.FormatRate(neckArrRate)})" +
                $" - vet: {neckVetDetections.Count}/{INs} ({this.FormatRate(neckVetRate)})");
            sb.AppendLine($"overall - arr: {this.FormatRate(overallArrRate)}" +
                $" - vet: {this.FormatRate(overallVetRate)}" +
                $" - average: {this.FormatRate(overallAverage)}");
        }
        var arrAverage = overallArrRates.Sum() / overallArrRates.Count;
        var vetAverage = overallVetRates.Sum() / overallVetRates.Count;
        var average = (arrAverage + vetAverage) / 2;

        sb.AppendLine("TOTAL ".PadRight(150, '='));
        sb.AppendLine($"arr: {this.FormatRate(arrAverage)}");
        sb.AppendLine($"vet: {this.FormatRate(vetAverage)}");
        sb.AppendLine($"total: {this.FormatRate(average)}");

        this.popupService.RenderValidation(sb.ToString());
    }

    private string FormatRate(double rate)
        => $"{rate * 100:0.##}%";
}
