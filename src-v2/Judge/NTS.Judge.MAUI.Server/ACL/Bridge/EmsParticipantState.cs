﻿using NTS.Judge.MAUI.Server.ACL.EMS;

namespace NTS.Judge.MAUI.Server.ACL.Bridge;

public class EmsParticipantState : IEmsParticipantState
{
    public bool Unranked { get; set; }
    public string Number { get; set; }
    public int? MaxAverageSpeedInKmPh { get; set; }
    public int Id { get; set; }
}