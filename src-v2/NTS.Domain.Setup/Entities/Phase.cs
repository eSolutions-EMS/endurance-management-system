using NTS.Domain.Setup.Import;
using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;

public class Phase : DomainEntity, ISummarizable, IImportable
{
    public static Phase Create(Loop loop, int recovery, int rest) => new(loop, recovery, rest);

    public static Phase Update(int id, Loop loop, int recovery, int rest) => new(id, loop, recovery, rest);

    [JsonConstructor]
    public Phase(int id, Loop loop, int recovery, int rest)
    {
        Id = id;
        Loop = loop;
        Recovery = recovery;
        Rest = rest;
    }
    public Phase(Loop loop, int recovery, int rest)
    {
        if (recovery <= 0)
        {
            throw new DomainException(nameof(Recovery), "Recovery time cannot be zero or less.");
        }
        if (rest <= 0)
        {
            throw new DomainException(nameof(Rest), "Rest duration cannot be zero or less.");
        }
        Loop = loop;
        Recovery = recovery;
		Rest = rest;
	}

    public Loop? Loop { get; internal set; }
    public int Recovery { get; }
    public int Rest { get; }

    public override string ToString()
    {
        var min = "min".Localize();
		var rec = "recovery".Localize();
        var loop = "loop".Localize();
		var rest = "Rest".Localize();
        return Combine($"{loop}: {Loop}", $"{rec}: {Recovery}{min}", $"{rest}: {Rest}{min}");
    }
}
