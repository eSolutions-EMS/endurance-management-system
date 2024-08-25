using NTS.Compatibility.EMS.Entities.EMS;
using NTS.Domain;
using NTS.Domain.Enums;
using NTS.Domain.Objects;

namespace NTS.Judge.ACL.Factories;

public class SnapshotFactory
{
    public static Snapshot Create(EmsParticipantEntry participant, EmsWitnessEventType emsType, bool isFinal)
    {
        var number = int.Parse(participant.Number);
        var method = SnapshotMethod.EmsIntegration;
        var timestamp = new Timestamp(participant.ArriveTime!.Value);
        
        return emsType switch
        {
            EmsWitnessEventType.VetIn => new VetgateSnapshot(number, method, timestamp),
            EmsWitnessEventType.Arrival => isFinal
                ? new FinishSnapshot(number, method, timestamp)
                : new StageSnapshot(number, method, timestamp),
            _ => throw new Exception($"Invalid WitnessEventType for participant '{participant.Number}'"),
        };
    }
}
