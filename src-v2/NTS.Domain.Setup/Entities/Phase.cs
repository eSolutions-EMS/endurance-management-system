using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Phase : DomainEntity, ISummarizable, IImportable
{
    public static Phase Create(Loop? loop, int recovery, int rest) => new(loop, recovery, rest);

    public static Phase Update(int id, Loop? loop, int recovery, int rest) => new(id, loop, recovery, rest);

    [JsonConstructor]
    public Phase(int id, Loop? loop, int recovery, int rest) : base(id)
    {
        Loop = Required(nameof(Loop), loop);
        Recovery = recovery;
        Rest = rest;
    }

    public Phase(Loop? loop, int recovery, int rest) : this(
        GenerateId(),
        Required(nameof(Loop), loop),
        PositiveMinutes(nameof(Recovery), recovery),
        PositiveMinutes(nameof(Rest), rest))
    {
	}

    static int PositiveMinutes(string property, int minutes)
    {
        if (minutes <= 0)
        {
            throw new DomainException(property, "Duration cannot be less than 1 minute");
        }
        return minutes;
    }

    public Loop? Loop { get; }
    public int Recovery { get; }
    public int Rest { get; }

    public override string ToString()
    {
        var min = "min".Localize();
		var rec = "recovery".Localize();
        var loop = "loop".Localize();
		var rest = "Rest".Localize();
        return Combine(
            $"{loop}: {Loop}",
            $"{rec}: {Recovery}{min}",
            $"{rest}: {Rest}{min}");
    }
}
