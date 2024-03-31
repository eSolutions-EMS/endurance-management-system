using NTS.Domain.Setup.Import;

namespace NTS.Domain.Setup.Entities;

public class Loop : DomainEntity, ISummarizable
{
    public Loop(double distance, int rec, int rest, bool isFinal)
    {
		this.Distance = distance;
		this.Rec = rec;
		this.Rest = rest;
		this.IsFinal = isFinal;
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
