namespace EMS.Witness.Services;

public interface IPermissionsService
{
    Task<bool> HasNetworkPermissions();
}
