using NTS.Domain.Setup.Import;
using System.Text.Json.Serialization;

namespace NTS.Domain.Setup.Entities;

public class Loop : DomainEntity, ISummarizable
{
    public static Loop Create(double distance, int rec, int rest, bool isFinal) => new(distance, rec, rest, isFinal);

    public static Loop Update(int id, double distance, int rec, int rest, bool isFinal) => new(id, distance, rec, rest, isFinal);

    [JsonConstructor]
    public Loop(int id, double distance, int rec, int rest, bool isFinal)
    {
        Id = id;
        Distance = distance;
        Rec = rec;
        Rest = rest;
        IsFinal = isFinal;
    }
    public Loop(double distance, int rec, int rest, bool isFinal)
    {
		Distance = distance;
		Rec = rec;
		Rest = rest;
		IsFinal = isFinal;
	}

    public double Distance { get; private set; }
	public int Rec { get; private set; }
	public int Rest { get; private set; }
	public bool IsFinal { get; private set; }

	public override string ToString()
	{
		var km = "km".Localize();
		var min = "min".Localize();
		var final = "final".Localize();
		var rec = "Rec".Localize();
		var rest = "Rest".Localize();
		return $"{this.Distance}{km} {rec}: {this.Rec}{min} {rest}: {this.Rest}{min}" + (this.IsFinal ? final : string.Empty);
	}
}
