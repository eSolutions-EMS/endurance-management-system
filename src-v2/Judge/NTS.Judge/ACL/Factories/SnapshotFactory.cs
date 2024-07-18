using NTS.Compatibility.EMS.Entities.EMS;
using NTS.Domain.Enums;
using NTS.Domain.Objects;

namespace NTS.Judge.ACL.Factories;

public class SnapshotFactory
{
    public static Snapshot Create(EmsParticipantEntry participant, EmsWitnessEventType emsType, bool isFinal)
    {
        var number = int.Parse(participant.Number);
        var type = emsType switch
        {
            EmsWitnessEventType.VetIn => SnapshotType.Vet,
            EmsWitnessEventType.Arrival => isFinal
                ? SnapshotType.Final
                : SnapshotType.Stage,
            _ => throw new Exception($"Invalid WitnessEventType for participant '{participant.Number}'"),
        };

        return new Snapshot(number, type, SnapshotMethod.EmsIntegration);
    }
}
