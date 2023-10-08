using Core.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Core.Domain.AggregateRoots.Manager.Aggregates.Participants;

public class ParticipantsBatch : IEquatable<ParticipantsBatch>, IComparable<ParticipantsBatch>
{
    public ParticipantsBatch(WitnessEventType type, IEnumerable<ParticipantEntry> participants)
    {
        this.SendAt = DateUtilities.Now;
        this.Type = type;
        this.Participants = new ReadOnlyCollection<ParticipantEntry>(participants.ToList());
    }


    public DateTime SendAt { get; internal set; }
    public WitnessEventType Type { get; internal set; }
    public IReadOnlyList<ParticipantEntry> Participants { get; }

    public override string ToString()
    {
        var time = DateUtilities.FormatTime(this.SendAt).StripMilliseconds();
        return $"{time} - {this.Type}: {this.Participants.Count}";
    }

    public int CompareTo(ParticipantsBatch other)
    {
        if (this.SendAt > other.SendAt)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    public bool Equals(ParticipantsBatch other)
    {
        return this.SendAt == other.SendAt && this.Type == other.Type;
    }

    public override int GetHashCode()
    {
        return this.SendAt.GetHashCode() + this.Type.GetHashCode();
    }
}
