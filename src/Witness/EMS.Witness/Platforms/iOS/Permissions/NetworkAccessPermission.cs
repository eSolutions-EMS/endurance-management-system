namespace EMS.Witness.Platforms.iOS.Permissions;
public class NetworkAccessPermission : Microsoft.Maui.ApplicationModel.Permissions.BasePlatformPermission
{
	protected override Func<IEnumerable<string>> RequiredInfoPlistKeys 
		=> () => new string[] { "NSLocalNetworkUsageDescription" };
}
