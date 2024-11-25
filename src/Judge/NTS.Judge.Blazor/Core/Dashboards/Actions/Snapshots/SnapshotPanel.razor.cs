using MudBlazor;
using NTS.Domain.Objects;

namespace NTS.Judge.Blazor.Core.Dashboards.Actions.Snapshots;

public partial class SnapshotPanel
{
	const string DEFAULT_TIME = "00:00:00";
    static readonly PatternMask TIME_MASK = new("00:00:00");

	string _time = DEFAULT_TIME;

	[Inject]
	IManualProcessor ManualProcessor { get; set; } = default!;

	void Snapshot()
	{
		var currentTime = DateTime.Now.TimeOfDay;
		_time = currentTime.ToString();
	}

	void Process()
	{
		if (_time == DEFAULT_TIME)
		{
			return;
		}

		var inputTime = TimeSpan.Parse(_time);
		var time = DateTime.Today + inputTime;
		var timestamp = new Timestamp(time);
		ManualProcessor.Process(timestamp);
	}
}
