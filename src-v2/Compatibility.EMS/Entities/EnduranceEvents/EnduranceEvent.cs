using NTS.Compatibility.EMS.Abstractions;
using NTS.Compatibility.EMS.Entities.Competitions;
using NTS.Compatibility.EMS.Entities.Countries;
using NTS.Compatibility.EMS.Entities.Personnels;

namespace NTS.Compatibility.EMS.Entities.EnduranceEvents;

public class EnduranceEvent : DomainBase<EnduranceEventException>
{
    private EnduranceEvent()  { }
    internal EnduranceEvent(string name, Country country, string feiId, string feiCode, string showFeiId) : this(name, country)
    {
        FeiId = feiId;
        FeiCode = feiCode;
        ShowFeiId = showFeiId;
    }
    internal EnduranceEvent(string name, Country country) : base(GENERATE_ID)
    {
        this.Name = name;
        this.Country = country;
    }

    private List<Personnel> membersOfVetCommittee = new();
    private List<Personnel> membersOfJudgeCommittee = new();
    private List<Personnel> stewards = new();
    private List<Competition> competitions = new();

    public string FeiCode { get; internal set; }
    public string ShowFeiId { get; internal set; }
    public string FeiId { get; internal set; }
    public string Name { get; internal set; }
    public string PopulatedPlace { get; internal set; }
    public bool HasStarted { get; internal set; }
    public Country Country { get; internal set; }
    public Personnel PresidentGroundJury { get; internal set; }
    public Personnel PresidentVetCommittee { get; internal set; }
    public Personnel ForeignJudge { get; internal set; }
    public Personnel FeiTechDelegate { get; internal set; }
    public Personnel FeiVetDelegate { get; internal set; }
    public Personnel ActiveVet { get; internal set; }

    public void Save(Competition competition)
    {
        throw new NotImplementedException();
    }
    public void Save(Personnel personnel)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Personnel> MembersOfVetCommittee
    {
        get => this.membersOfVetCommittee.AsReadOnly();
        private set => this.membersOfVetCommittee = value.ToList();
    }
    public IReadOnlyList<Personnel> MembersOfJudgeCommittee
    {
        get => this.membersOfJudgeCommittee.AsReadOnly();
        private set => this.membersOfJudgeCommittee = value.ToList();
    }
    public IReadOnlyList<Personnel> Stewards
    {
        get => this.stewards.AsReadOnly();
        private set => this.stewards = value.ToList();
    }
    public IReadOnlyList<Competition> Competitions
    {
        get => this.competitions.AsReadOnly();
        private set => this.competitions = value.ToList();
    }
}
