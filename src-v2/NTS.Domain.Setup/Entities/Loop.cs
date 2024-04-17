using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Loop : DomainEntity, ISummarizable
{
    public static Loop Create(double distance, int rec, int rest, bool isFinal) => new(distance, rec, rest, isFinal);

    public static Loop Update(int id, double distance, int rec, int rest, bool isFinal) => new(id, distance, rec, rest, isFinal);

    [JsonConstructor]
    public Loop(int id, double distance, int rec, int rest, bool isFinal)
    {
        if (distance <= 0)
        {
            throw new DomainException(nameof(Distance), "Distance cannot be zero or less.");
        }

        if (rec <= 0)
        {
            throw new DomainException(nameof(Recovery), "Recovery time cannot be zero or less.");
        }

        if (rest <= 0)
        {
            throw new DomainException(nameof(Rest), "Rest duration cannot be zero or less.");
        }

        Id = id;
        Distance = distance;
        Recovery = rec;
        Rest = rest;
        IsFinal = isFinal;
    }
    public Loop(double distance, int rec, int rest, bool isFinal)
    {
        if (distance <= 0)
        {
            throw new DomainException(nameof(Distance), "Distance cannot be zero or less.");
        }

        if (rec <= 0)
        {
            throw new DomainException(nameof(Recovery), "Recovery time cannot be zero or less.");
        }

        if (rest <= 0)
        {
            throw new DomainException(nameof(Rest), "Rest duration cannot be zero or less.");
        }

        Distance = distance;
		Recovery = rec;
		Rest = rest;
		IsFinal = isFinal;
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
