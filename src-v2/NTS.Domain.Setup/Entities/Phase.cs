using NTS.Domain.Setup.Entities;
using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

public class Phase : DomainEntity, ISummarizable, IImportable, IParent<Lap>
{
    public static Phase Create(int selectedLap, int recovery, int rest) => new(selectedLap, recovery, rest);

    public static Phase Update(int id, int selectedLap, int recovery, int rest) => new(id, selectedLap, recovery, rest);

    [JsonConstructor]
    public Phase(int id, int? selectedLap, int? recovery, int? rest)
    {
        Id = id;
        if(selectedLap != null)
        {
            SelectedLap = selectedLap;
        }
        if(recovery != null)
        {
            Recovery = recovery;
        }
        if(rest != null)
        {
            Rest = rest;
        }
    }

    public Phase() { }
    public Phase(int selectedLap, int recovery, int rest)
    {
        if (recovery <= 0)
        {
            throw new DomainException(nameof(Recovery), "Recovery time cannot be zero or less.");
        }
        if (rest <= 0)
        {
            throw new DomainException(nameof(Rest), "Rest duration cannot be zero or less.");
        }
        SelectedLap = selectedLap;
        Recovery = recovery;
		Rest = rest;
	}

    public int? SelectedLap { get; set; }
    public int? Recovery { get; private set; }
    public int? Rest { get; private set; }
    public static List<Lap> Laps { get; set; } = new();

    public void Add(Lap child)
    {
        Laps.Add(child);
    }
    public void Remove(Lap child)
    {
        Laps.Remove(child);
    }
    public void Update(Lap child)
    {
        Laps.RemoveAll(lap => lap.Id == child.Id);
        Laps.Add(child);
    }

    public override string ToString()
    {
        var min = "min".Localize();
		var rec = "Recovery".Localize();
        var phase = "Phase".Localize();
		var rest = "Rest".Localize();
		return $"{phase} -> {Laps[(int)SelectedLap!]} {rec}: {Recovery}{min} {rest}: {Rest}{min}";
    }
}
