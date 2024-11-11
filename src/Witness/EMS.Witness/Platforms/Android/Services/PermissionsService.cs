namespace EMS.Witness.Platforms.Services
{
    public class PermissionsService : IPermissionsService
    {
        public Task<bool> HasNetworkPermissions() => Task.FromResult(true);
    }
}
