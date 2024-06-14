using NTS.Domain.Setup.Entities;
using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

public class Phase : DomainEntity, ISummarizable, IImportable
{
    public static Phase Create(Lap lap, int recovery, int rest) => new(lap, recovery, rest);

    public static Phase Update(int id, Lap lap, int recovery, int rest) => new(id, lap, recovery, rest);

    [JsonConstructor]
    public Phase(int id, Lap selectedLap, int recovery, int rest)
    {
        Id = id;
        Lap = selectedLap;
        Recovery = recovery;
        Rest = rest;
    }
    public Phase(Lap selectedLap, int recovery, int rest)
    {
        if (recovery <= 0)
        {
            throw new DomainException(nameof(Recovery), "Recovery time cannot be zero or less.");
        }
        if (rest <= 0)
        {
            throw new DomainException(nameof(Rest), "Rest duration cannot be zero or less.");
        }
        Lap = selectedLap;
        Recovery = recovery;
		Rest = rest;
	}
    public Lap? Lap { get; set; }
    public int Recovery { get; private set; }
    public int Rest { get; private set; }

    public override string ToString()
    {
        var min = "min".Localize();
		var rec = "Recovery".Localize();
        var lap = "Lap".Localize();
        var phase = "Phase".Localize();
		var rest = "Rest".Localize();
		return $"{phase} -> {lap}: {Lap} {rec}: {Recovery}{min} {rest}: {Rest}{min}";
    }
}
