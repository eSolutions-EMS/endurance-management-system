using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Loop : DomainEntity, ISummarizable
{
    public static Loop Create(double distance, int rec, int rest) => new(distance, rec, rest);

    public static Loop Update(int id, double distance, int rec, int rest) => new(id, distance, rec, rest);

    [JsonConstructor]
    public Loop(int id, double distance, int recovery, int rest) : this(distance, recovery, rest)
    {
        Id = id;
    }
    public Loop(double distance, int recovery, int rest)
    {
        if (distance <= 0)
        {
            throw new DomainException(nameof(Distance), "Distance cannot be zero or less.");
        }
        if (recovery <= 0)
        {
            throw new DomainException(nameof(Recovery), "Recovery time cannot be zero or less.");
        }
        if (rest <= 0)
        {
            throw new DomainException(nameof(Rest), "Rest duration cannot be zero or less.");
        }

        Distance = distance;
		Recovery = recovery;
		Rest = rest;
	}

    public double Distance { get; private set; }
	public int Recovery { get; private set; }
	public int Rest { get; private set; }
	public bool IsFinal { get; private set; }

	public override string ToString()
	{
		var km = "km".Localize();
		var min = "min".Localize();
		var final = "final".Localize();
		var rec = "Recovery".Localize();
		var rest = "Rest".Localize();
		return $"{this.Distance}{km} {rec}: {this.Recovery}{min} {rest}: {this.Rest}{min}" + (this.IsFinal ? final : string.Empty);
	}
}
