using System.ComponentModel;

namespace NTS.Domain.Enums;

public enum OfficialRole
{
    Steward = 1,

    [Description("Chief steward")]
    ChiefSteward = 2,
    
    [Description("Veterinary Commission")]
    VeterinaryCommission = 3,

    [Description("President of Veterinary Commission")]
    VeterinaryCommissionPresident = 4,

    [Description("Ground jury")]
    GroundJury = 5,

    [Description("President of Ground Jury")]
    GroundJuryPresident = 6,

    [Description("Technical delegate")]
    TechnicalDelegate = 7,

    [Description("Foreign judge")]
    ForeignJudge = 8,
    
    [Description("Foreign Veterinary Delegate")]
    ForeignVeterinaryDelegate = 9
}