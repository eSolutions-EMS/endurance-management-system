using NTS.Domain.Setup.Entities;
using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

public class Phase : DomainEntity, ISummarizable, IImportable
{
    public static Phase Create(double distance, int recovery, int rest) => new(distance, recovery, rest);

    public static Phase Update(int id, double distance, int recovery, int rest) => new(id, distance, recovery, rest);

    [JsonConstructor]
    public Phase(int id, Loop loop, int recovery, int rest)
    {
        Id = id;
        Loop = loop;
        Recovery = recovery;
        Rest = rest;
    }

    public Phase(int id, double distance, int recovery, int rest) : this(distance, recovery, rest)
    {
        Id = id;
    }
    public Phase(double distance, int recovery, int rest)
    {
        if (distance <= 0)
        {
            throw new DomainException(nameof(Loop), "Phase distance cannot be zero or less.");
        }
        if (recovery <= 0)
        {
            throw new DomainException(nameof(Recovery), "Recovery time cannot be zero or less.");
        }
        if (rest <= 0)
        {
            throw new DomainException(nameof(Rest), "Rest duration cannot be zero or less.");
        }

        Loop = new Loop(distance);
		Recovery = recovery;
		Rest = rest;
	}

    public Loop Loop { get; private set; }
	public int Recovery { get; private set; }
    public int Rest { get; private set; }

    public override string ToString()
    {
        var km = "km".Localize();
        var min = "min".Localize();
		var rec = "Recovery".Localize();
        var phase = "Phase".Localize();
		var rest = "Rest".Localize();
		return $"Loop -> {phase}{Loop.Distance}{km} {rec}: {Recovery}{min} {rest}: {Rest}{min}";
    }
}
