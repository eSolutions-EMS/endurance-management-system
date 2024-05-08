using NTS.Domain.Setup.Entities;
using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

public class Loop : DomainEntity, ISummarizable, IImportable
{
    public static Loop Create(double distance, int recovery, int rest) => new(distance, recovery, rest);

    public static Loop Update(int id, double distance, int recovery, int rest) => new(id, distance, recovery, rest);

    [JsonConstructor]
    public Loop(int id, Phase phase, int recovery, int rest)
    {
        Id = id;
        Phase = phase;
        Recovery = recovery;
        Rest = rest;
    }

    public Loop(int id, double distance, int recovery, int rest) : this(distance, recovery, rest)
    {
        Id = id;
    }
    public Loop(double distance, int recovery, int rest)
    {
        if (distance <= 0)
        {
            throw new DomainException(nameof(Phase), "Phase distance cannot be zero or less.");
        }
        if (recovery <= 0)
        {
            throw new DomainException(nameof(Recovery), "Recovery time cannot be zero or less.");
        }
        if (rest <= 0)
        {
            throw new DomainException(nameof(Rest), "Rest duration cannot be zero or less.");
        }

        Phase = new Phase(distance);
		Recovery = recovery;
		Rest = rest;
	}

    public Phase Phase { get; private set; }
	public int Recovery { get; private set; }
    public int Rest { get; private set; }

    public override string ToString()
    {
        var km = "km".Localize();
        var min = "min".Localize();
		var rec = "Recovery".Localize();
        var phase = "Phase".Localize();
		var rest = "Rest".Localize();
		return $"Loop -> {phase}{Phase.Distance}{km} {rec}: {Recovery}{min} {rest}: {Rest}{min}";
    }
}
