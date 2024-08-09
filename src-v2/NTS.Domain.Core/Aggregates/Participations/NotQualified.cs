using System.ComponentModel;

namespace NTS.Domain.Core.Aggregates.Participations;

public record Withdrawn : NotQualified
{
    public override string ToString()
    {
        return "WD";
    }
}

public record Retired : NotQualified
{
    public override string ToString()
    {
        return "RET";
    }
}

public record Disqualified : NotQualified
{
    private Disqualified()
    {
    }
    public Disqualified(string complement) : base(complement)
    {
    }

    public override string ToString()
    {
        return $"DQ";
    }
}

public record FinishedNotRanked : NotQualified
{
    private FinishedNotRanked()
    {
    }
    public FinishedNotRanked(string complement) : base(complement)
    {
    }

    public override string ToString()
    {
        return $"FNR";
    }
}

public record FailedToQualify : NotQualified
{
    private FailedToQualify()
    {
    }
    public FailedToQualify(params FTQCodes[] codes)
    {
        if (codes.Contains(FTQCodes.FTC))
        {
            throw new DomainException($"'Failed to Complete' requires a writen explanation from officials. Please provide 'complement'");
        }
        Codes = codes;
    }
    public FailedToQualify(string complement) : base(complement)
    {
        Codes.ToList().Add(FTQCodes.FTC);
    }

    public IEnumerable<FTQCodes> Codes { get; private set; }

    public override string ToString()
    {
        return $"FTQ {Codes}";
    }
}

public enum FTQCodes
{
    /// <summary>
    /// Not respecting applicable speed restrictions
    /// </summary>
    [Description("Not respecting applicable speed restrictions")]
    SP = 1,
    /// <summary>
    /// Irregular Gait
    /// </summary>
    [Description("Irregular Gait")]
    GA = 2,
    /// <summary>
    /// Metabolic issue
    /// </summary>
    [Description("Metabolic issue")]
    ME = 3,
    /// <summary>
    /// Minor injury
    /// </summary>
    [Description("Minor injury")]
    MI = 4,
    /// <summary>
    /// Serious injury (musculoskeletal)
    /// </summary>
    [Description("Serious injury (musculoskeletal)")]
    SIMUSCU = 5,
    /// <summary>
    /// Serious injury (metabolic)
    /// </summary>
    [Description("Serious injury (metabolic)")]
    SIMETA = 6,
    /// <summary>
    /// Catastrophic injury
    /// </summary>
    [Description("Catastrophic injury")]
    CI = 7,
    /// <summary>
    /// Out of time
    /// </summary>
    [Description("Out of time")]
    OT = 8,
    /// <summary>
    /// Failed to complete a Loop, but passes Horse inspection after that Loop. <seealso cref="FailedToQualify.Complement"/> is required
    /// </summary>
    [Description("Failed to complete a Loop, but passes Horse inspection after that Loop.")]
    FTC = 9
}

public abstract record NotQualified : DomainObject
{
    protected NotQualified()
    {
    }
    protected NotQualified(string complement)
    {
        Complement = complement;
    }

    public string? Complement { get; private set; }
}
