using NTS.Domain.Setup.Entities;
using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

public class Phase : DomainEntity, ISummarizable, IImportable
{
    public static Phase Create(Loop Loop, int recovery, int rest) => new(Loop, recovery, rest);

    public static Phase Update(int id, Loop Loop, int recovery, int rest) => new(id, Loop, recovery, rest);

    [JsonConstructor]
    public Phase(int id, Loop selectedLoop, int recovery, int rest)
    {
        Id = id;
        Loop = selectedLoop;
        Recovery = recovery;
        Rest = rest;
    }
    public Phase(Loop selectedLoop, int recovery, int rest)
    {
        if (recovery <= 0)
        {
            throw new DomainException(nameof(Recovery), "Recovery time cannot be zero or less.");
        }
        if (rest <= 0)
        {
            throw new DomainException(nameof(Rest), "Rest duration cannot be zero or less.");
        }
        Loop = selectedLoop;
        Recovery = recovery;
		Rest = rest;
	}
    public Loop? Loop { get; set; }
    public int Recovery { get; private set; }
    public int Rest { get; private set; }

    public override string ToString()
    {
        var min = "min".Localize();
		var rec = "Recovery".Localize();
        var loop = "Loop".Localize();
        var phase = "Phase".Localize();
		var rest = "Rest".Localize();
		return $"{phase} -> {loop}: {Loop} {rec}: {Recovery}{min} {rest}: {Rest}{min}";
    }
}
