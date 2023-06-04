namespace EMS.Witness.Platforms.Services;

public interface IPermissionsService
{
    Task<bool> HasNetworkPermissions();
}
