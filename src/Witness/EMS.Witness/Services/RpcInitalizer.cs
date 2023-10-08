using Core.Application.Rpc;
using Core.ConventionalServices;
using EMS.Witness.Platforms.Services;
using EMS.Witness.Shared.Toasts;

namespace EMS.Witness.Services;

public class RpcInitalizer : IRpcInitalizer
{
	private readonly IToaster toaster;
	private readonly IEnumerable<IRpcClient> rpcClients;
	private readonly IPermissionsService permissionsService;
	private readonly WitnessContext context;
	private bool isHandshaking;

	public RpcInitalizer(
		IWitnessContext context,
		IEnumerable<IRpcClient> rpcClients,
		IPermissionsService permissionsService,
		IToaster toaster)
    {
		this.context = (WitnessContext)context;
		this.rpcClients = rpcClients;
		this.permissionsService = permissionsService;
		this.toaster = toaster;
    }

    public async Task StartConnections()
	{
		if (!await this.permissionsService.HasNetworkPermissions())
		{
            this.toaster.Add(
                "Network permission rejected",
                "eWitness app cannot operate without Network permissions. Grant permissions in device settings.",
                UiColor.Danger);
			return;
        }
		this.isHandshaking = true;
		try
		{
            foreach (var client in this.rpcClients.Where(x => !x.IsConnected))
            {
                await client.Connect();
            }
        }
		catch (Exception exception)
		{
			this.ToastError(exception);
		}
		finally
		{
			this.isHandshaking = false;
			this.context.RaiseIsHandshakingEvent(this.isHandshaking);
		}
	}

	private void ToastError(Exception exception)
	{
		this.toaster.Add(exception.Message, exception?.StackTrace, UiColor.Danger, 30);
	}
}

public interface IRpcInitalizer : ISingletonService
{
	Task StartConnections();
}
