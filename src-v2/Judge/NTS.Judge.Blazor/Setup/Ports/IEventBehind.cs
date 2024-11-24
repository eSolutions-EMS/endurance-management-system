﻿using Not.Blazor.CRUD.Forms.Ports;
using Not.Blazor.Ports;
using NTS.Judge.Blazor.Setup.Events;

namespace NTS.Judge.Blazor.Setup.Ports;

public interface IEnduranceEventBehind : IFormBehind<EnduranceEventFormModel>, IObservableBehind
{
    EnduranceEventFormModel? Model { get; }
}