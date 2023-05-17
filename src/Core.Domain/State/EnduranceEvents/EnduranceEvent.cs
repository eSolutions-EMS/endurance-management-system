using Core.Domain.Core.Models;
using Core.Domain.Enums;
using Core.Domain.State.Competitions;
using Core.Domain.State.Countries;
using Core.Domain.State.Personnels;
using Core.Domain.Core.Extensions;
using Core.Domain.State.Participations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.State.EnduranceEvents;

public class EnduranceEvent : DomainBase<EnduranceEventException>, IEnduranceEventState
{
    private EnduranceEvent()  {}
    internal EnduranceEvent(string name, Country country) : base(GENERATE_ID)
    {
        this.Name = name;
        this.Country = country;
    }

    private List<Personnel> membersOfVetCommittee = new();
    private List<Personnel> membersOfJudgeCommittee = new();
    private List<Personnel> stewards = new();
    private List<Competition> competitions = new();

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
        this.competitions.AddOrUpdate(competition);
    }
    public void Save(Personnel personnel)
    {
        switch (personnel.Role)
        {
            case PersonnelRole.PresidentGroundJury:
                this.PresidentGroundJury = personnel;
                break;
            case PersonnelRole.PresidentVetCommission:
                this.PresidentVetCommittee = personnel;
                break;
            case PersonnelRole.ForeignJudge:
                this.ForeignJudge = personnel;
                break;
            case PersonnelRole.FeiTechDelegate:
                this.FeiTechDelegate = personnel;
                break;
            case PersonnelRole.FeiVetDelegate:
                this.FeiVetDelegate = personnel;
                break;
            case PersonnelRole.ActiveVet:
                this.ActiveVet = personnel;
                break;
            case PersonnelRole.MemberOfVetCommittee:
                this.membersOfVetCommittee.AddOrUpdate(personnel);
                break;
            case PersonnelRole.MemberOfJudgeCommittee:
                this.membersOfJudgeCommittee.AddOrUpdate(personnel);
                break;
            case PersonnelRole.Steward:
                this.stewards.AddOrUpdate(personnel);
                break;
            case PersonnelRole.Invalid:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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
