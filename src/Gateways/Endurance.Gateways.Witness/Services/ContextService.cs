namespace Endurance.Gateways.Witness.Services;

public class ContextService : IContext
{
	public string? ApiHost { get; set; }
}

public interface IContext
{
	string? ApiHost { get; }
}
