﻿using Newtonsoft.Json;
using NTS.Domain.Setup.Import;

namespace NTS.Domain.Setup.Entities;

public class Combination : DomainEntity, ISummarizable, IImportable, IParent
{
    public static Combination Create(int number, Athlete? athlete, Horse? horse, Tag? tag)
    {
        return new(number, athlete, horse, tag);
    }

    public static Combination Update(int id, int number, Athlete? athlete, Horse? horse, Tag? tag)
    {
        return new(id, number, athlete, horse, tag);
    }

    [JsonConstructor]
    public Combination(int id, int number, Athlete? athlete, Horse? horse, Tag? tag)
        : base(id)
    {
        Number = NotDefault(nameof(Number), number);
        Athlete = Required(nameof(Athlete), athlete);
        Horse = Required(nameof(Horse), horse);
        Tag = tag;
    }

    public Combination(int number, Athlete? athlete, Horse? horse, Tag? tag)
        : this(GenerateId(), number, athlete, horse, tag) { }

    public int Number { get; }
    public Athlete Athlete { get; }
    public Horse Horse { get; }
    public Tag? Tag { get; }

    public string Summarize()
    {
        var summary = new Summarizer(this);
        return summary.ToString();
    }

    public override string ToString()
    {
        var number = $"{"#".Localize()}{Number}";
        return Combine(number, Athlete, Horse);
    }
}
