namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsHorse : EmsDomainBase<EmsHorseException>, IEmsHorseState
{
    private EmsHorse() { }
    public EmsHorse(string feiId, string name, string breed, string club) : base(default)
    {
        Name = name;
        Breed = breed;
        FeiId = feiId;
        Club = club;
    }

    public EmsHorse(
        string feiId,
        string name,
        bool isStallion,
        string breed,
        string trainerFeiId,
        string trainerFirstName,
        string trainerLastName) : base(default)
    {
        Name = name;
        Breed = breed;
        IsStallion = isStallion;
        FeiId = feiId;
        TrainerFeiId = trainerFeiId;
        TrainerFirstName = trainerFirstName;
        TrainerLastName = trainerLastName;
    }

    public EmsHorse(IEmsHorseState state) : base(default)
    {
        FeiId = state.FeiId;
        Club = state.Club;
        IsStallion = state.IsStallion;
        Breed = state.Breed;
        TrainerFeiId = state.TrainerFeiId;
        TrainerFirstName = state.TrainerFirstName;
        TrainerLastName = state.TrainerLastName;
        Name = Validator.IsRequired(state.Name, "Name");
    }

    public string FeiId { get; internal set; }
    public string Name { get; internal set; }
    public string Club { get; internal set; }
    public bool IsStallion { get; internal set; }
    public string Breed { get; internal set; }
    public string TrainerFeiId { get; internal set; }
    public string TrainerFirstName { get; internal set; }
    public string TrainerLastName { get; internal set; }

    public string TrainerName => $"{TrainerFirstName} {TrainerLastName}";
}
