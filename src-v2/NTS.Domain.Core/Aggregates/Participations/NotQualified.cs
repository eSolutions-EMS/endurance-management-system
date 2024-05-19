using Not.Events;

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
    public FailedToQualify(FTQCodes code)
    {
        if (code == FTQCodes.FTC)
        {
            throw new DomainException($"'Filed to Complete' requires a writen explanation from officials. Please provide 'complement'");
        }
        Code = code;
    }
    public FailedToQualify(string complement) : base(complement)
    {
        Code = FTQCodes.FTC;
    }

    public FTQCodes Code { get; }

    public override string ToString()
    {
        return Code.ToString();
    }
}

public enum FTQCodes
{
    /// <summary>
    /// Not respecting applicable speed restrictions
    /// </summary>
    SP = 1,
    /// <summary>
    /// Irregular Gait
    /// </summary>
    GA = 2,
    /// <summary>
    /// Metabolic issue
    /// </summary>
    ME = 3,
    /// <summary>
    /// Minor injury
    /// </summary>
    MI = 4,
    /// <summary>
    /// Serious injury (musculoskeletal)
    /// </summary>
    SIMUSCU = 5,
    /// <summary>
    /// Serious injury (metabolic)
    /// </summary>
    SIMETA = 6,
    /// <summary>
    /// Catastrophic injury
    /// </summary>
    CI = 7,
    /// <summary>
    /// Out of time
    /// </summary>
    OT = 8,
    /// <summary>
    /// Failed to complete a Loop, but passes Horse inspection after that Loop. <seealso cref="FailedToQualify.Complement"/> is required
    /// </summary>
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

    public string? Complement { get; }
}