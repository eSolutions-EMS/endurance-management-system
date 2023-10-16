﻿using System.ComponentModel.DataAnnotations;

namespace EMS.Judge.Setup.Models;

public class EventCreateModel
{
    [Required]
    public string? Place { get; set; }
    [Required]
    public string? CountryName { get; set; }
}
