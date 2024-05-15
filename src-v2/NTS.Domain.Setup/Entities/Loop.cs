using Newtonsoft.Json;

namespace NTS.Domain.Setup.Entities;
public class Loop : DomainEntity, IParent<Lap>
{
    public static Loop Create(double distance) => new(distance);
    public static Loop Update(int id, double distance) => new(id, distance);

    public Loop()
    {
        Laps = new List<Lap> { };
    }
    [JsonConstructor]
    public Loop(List<Lap> laps)
    {
        Laps = laps;
    }
    public Loop(Lap lap)
    {
        Laps.Add(lap);
    }
    public Loop (double distance)
    {
        var newLap = new Lap(distance);
        Laps.Add(newLap);
    }
    public Loop(int id, double distance) 
    {
        Id = id;
        Laps.RemoveAll(lap => lap.Id == id);
        var updatedLap = new Lap(distance);
        Laps.Add(updatedLap);
    }
    public List<Lap> Laps { get; set; } = default!;
    public override string ToString() 
    {
        var phase = "Loop".Localize();
        var sb = new StringBuilder();
        sb.Append($"{phase} -> {Laps}km long ");
        return sb.ToString();
    }

    public void Add(Lap child)
    {
        Laps.Add(child);
    }

    public void Remove(Lap child)
    {
        Laps.Remove(child);
    }

    public void Update(Lap child)
    {
        Laps.RemoveAll(lap => lap.Id == child.Id);
        Laps.Add(child);
    }
}
