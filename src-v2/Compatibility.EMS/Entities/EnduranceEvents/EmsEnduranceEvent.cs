using NTS.Compatibility.EMS.Abstractions;
using NTS.Compatibility.EMS.Entities.Competitions;
using NTS.Compatibility.EMS.Entities.Countries;
using NTS.Compatibility.EMS.Entities.Personnels;

namespace NTS.Compatibility.EMS.Entities.EnduranceEvents;

public class EmsEnduranceEvent : EmsDomainBase<EmsEnduranceEventException>
{
    [Newtonsoft.Json.JsonConstructor]
    private EmsEnduranceEvent() { }

    internal EmsEnduranceEvent(
        string name,
        EmsCountry country,
        string feiId,
        string feiCode,
        string showFeiId
    )
        : this(name, country)
    {
        FeiId = feiId;
        FeiCode = feiCode;
        ShowFeiId = showFeiId;
    }

    internal EmsEnduranceEvent(string name, EmsCountry country)
        : base(GENERATE_ID)
    {
        this.Name = name;
        this.Country = country;
    }

    private List<EmsPersonnel> membersOfVetCommittee = new();
    private List<EmsPersonnel> membersOfJudgeCommittee = new();
    private List<EmsPersonnel> stewards = new();
    private List<EmsCompetition> competitions = new();

    public string FeiCode { get; internal set; }
    public string ShowFeiId { get; internal set; }
    public string FeiId { get; internal set; }
    public string Name { get; internal set; }
    public string PopulatedPlace { get; internal set; }
    public bool HasStarted { get; internal set; }
    public EmsCountry Country { get; internal set; }
    public EmsPersonnel PresidentGroundJury { get; internal set; }
    public EmsPersonnel PresidentVetCommittee { get; internal set; }
    public EmsPersonnel ForeignJudge { get; internal set; }
    public EmsPersonnel FeiTechDelegate { get; internal set; }
    public EmsPersonnel FeiVetDelegate { get; internal set; }
    public EmsPersonnel ActiveVet { get; internal set; }

    public void Save(EmsCompetition competition)
    {
        throw new NotImplementedException();
    }

    public void Save(EmsPersonnel personnel)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<EmsPersonnel> MembersOfVetCommittee
    {
        get => this.membersOfVetCommittee.AsReadOnly();
        private set => this.membersOfVetCommittee = value.ToList();
    }
    public IReadOnlyList<EmsPersonnel> MembersOfJudgeCommittee
    {
        get => this.membersOfJudgeCommittee.AsReadOnly();
        private set => this.membersOfJudgeCommittee = value.ToList();
    }
    public IReadOnlyList<EmsPersonnel> Stewards
    {
        get => this.stewards.AsReadOnly();
        private set => this.stewards = value.ToList();
    }
    public IReadOnlyList<EmsCompetition> Competitions
    {
        get => this.competitions.AsReadOnly();
        private set => this.competitions = value.ToList();
    }
}
