using NTS.Compatibility.EMS.Entities.Horses;

namespace NTS.Judge.ACL.Bridge;

public class EmsHorseState : IEmsHorseState
{
    public string FeiId { get; set; }
    public string Name { get; set; }
    public string Club { get; set; }
    public bool IsStallion { get; set; }
    public string Breed { get; set; }
    public string TrainerFeiId { get; set; }
    public string TrainerFirstName { get; set; }
    public string TrainerLastName { get; set; }
    public int Id { get; set; }
}
