﻿using Newtonsoft.Json;
using System.ComponentModel;

namespace NTS.Domain.Core.Aggregates.Participations;

public record Withdrawn : NotQualified
{
    [JsonConstructor]
    public Withdrawn() : base(WITHDRAWN)
    {
    }
    public override string ToString()
    {
        return "WD";
    }
}

public record Retired : NotQualified
{
    [JsonConstructor]
    public Retired() : base(RETIRED) 
    {
    }
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
    [JsonConstructor]
    public Disqualified(string complement) : base(complement, DISQUALIFIED)
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
    [JsonConstructor]
    public FinishedNotRanked(string complement) : base(complement, FINISHED_NOT_RANKED)
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
        EliminationCode = FAILED_TO_QUALIFY;
    }

    public FailedToQualify(string complement ,params FTQCodes[] codes) : base(complement, FAILED_TO_QUALIFY)
    {
        Codes = codes;
        EliminationCode = FAILED_TO_QUALIFY;
    }
    [JsonConstructor]
    public FailedToQualify(string complement) : base(complement, FAILED_TO_QUALIFY)
    {
        Codes = new List<FTQCodes>{FTQCodes.FTC};
    }

    public IEnumerable<FTQCodes> Codes { get; private set; } = new List<FTQCodes>();

    public override string ToString()
    {
        string codes = String.Join('+', Codes);
        return $"FTQ {codes}";
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
    public const string WITHDRAWN = "WD";
    public const string RETIRED = "RET";
    public const string FINISHED_NOT_RANKED = "FNR";
    public const string DISQUALIFIED = "DQ";
    public const string FAILED_TO_QUALIFY = "FTQ";
    
    protected NotQualified()
    {
    }
    protected NotQualified(string eliminationCode)
    {
        EliminationCode = eliminationCode;
    }
    protected NotQualified(string complement, string eliminationCode)
    {
        Complement = complement;
        EliminationCode = eliminationCode;
    }

    public string EliminationCode { get; protected set; }
    public string? Complement { get; set; }
}