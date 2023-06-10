using EMS.Witness.Platforms.iOS.Permissions;
using MauiPermissions = Microsoft.Maui.ApplicationModel.Permissions;

namespace EMS.Witness.Platforms.Services;

public class PermissionsService : IPermissionsService
{
    public async Task<bool> HasNetworkPermissions()
    {
        var status = await MauiPermissions.CheckStatusAsync<NetworkAccessPermission>();
        return status == PermissionStatus.Granted;
    }
}
