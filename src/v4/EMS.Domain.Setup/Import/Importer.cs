using Common.Conventions;
using Common.Domain.Ports;
using EMS.Domain.Setup.Entities;
using EMS.Domain.Setup.Ports;

namespace EMS.Domain.Setup.Import;

public class Importer : IImporter
{
	private readonly IParser<Event> eventParser;
	private readonly IParser<StaffMember> personnelParser;
	private readonly IParser<Competition> competitionParser;
	private readonly IParser<Loop> loopParser;
	private readonly IParser<Starter> starterParser;
	private readonly IParser<Athlete> athleteParser;
	private readonly IParser<Horse> horseParser;
	private readonly IRepository<Event> eventRepository;
	private readonly IRepository<StaffMember> personnelRepository;
	private readonly IRepository<Competition> competitionRepository;
	private readonly IRepository<Loop> loopRepository;
	private readonly IRepository<Starter> starterRepository;
	private readonly IRepository<Athlete> athleteRepository;
	private readonly IRepository<Horse> horseRepository;

	public Importer(
        IParser<Event> eventParser,
        IParser<StaffMember> personnelParser,
        IParser<Competition> competitionParser,
        IParser<Loop> loopParser,
        IParser<Starter> starterParser,
        IParser<Athlete> athleteParser,
        IParser<Horse> horseParser,
		IRepository<Event> eventRepository,
		IRepository<StaffMember> personnelRepository,
		IRepository<Competition> competitionRepository,
		IRepository<Loop> loopRepository,
		IRepository<Starter> starterRepository,
		IRepository<Athlete> ahtleteRepository,
		IRepository<Horse> horseRepository)
	{
		this.eventParser = eventParser;
		this.personnelParser = personnelParser;
		this.competitionParser = competitionParser;
		this.loopParser = loopParser;
		this.starterParser = starterParser;
		this.athleteParser = athleteParser;
		this.horseParser = horseParser;
		this.eventRepository = eventRepository;
		this.personnelRepository = personnelRepository;
		this.competitionRepository = competitionRepository;
		this.loopRepository = loopRepository;
		this.starterRepository = starterRepository;
		this.athleteRepository = ahtleteRepository;
		this.horseRepository = horseRepository;
	}

	public async Task Import(string contents)
	{
		var @event = this.eventParser.Parse(contents).FirstOrDefault();
		var all = new List<Task>();
		if (@event is not null)
		{
			this.eventRepository.Create(@event).AddTo(all);
		}
		foreach (var personnel in this.personnelParser.Parse(contents))
		{
			this.personnelRepository.Create(personnel).AddTo(all);
		}
		foreach (var competition in this.competitionParser.Parse(contents))
		{
			this.competitionRepository.Create(competition).AddTo(all);
		}
		foreach (var loop in this.loopParser.Parse(contents))
		{
			this.loopRepository.Create(loop).AddTo(all);
		}
		foreach (var starter in this.starterParser.Parse(contents))
		{
			this.starterRepository.Create(starter).AddTo(all);
		}
		foreach (var athlete in this.athleteParser.Parse(contents))
		{
			this.athleteRepository.Create(athlete).AddTo(all);
		}
		foreach (var horse in this.horseParser.Parse(contents))
		{
			this.horseRepository.Create(horse).AddTo(all);
		}
		await Task.WhenAll(all);
	}
}

public interface IImporter : ITransientService
{
	Task Import(string contents);
}
