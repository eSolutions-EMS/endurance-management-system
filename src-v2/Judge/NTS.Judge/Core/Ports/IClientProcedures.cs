﻿using Not.Injection;
using NTS.Domain.Core.Objects.Payloads;

namespace NTS.Judge.Core.Ports;

public interface IClientProcedures : ITransient
{
    Task SendStartCreated(PhaseCompleted phaseCompleted);
    Task SendParticipationEliminated(ParticipationEliminated revoked);
    Task SendParticipationRestored(ParticipationRestored restored);
}