using System.ComponentModel.DataAnnotations;

namespace NTS.Domain.Enums;

public enum CompetitionType
{
    TypeNotSet = 0,
    [Display(Name = "Qualification")]
    Qualification = 1,
    [Display(Name = "Star Level")]
    Star = 2
}