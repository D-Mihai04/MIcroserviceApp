namespace DS.UserService.clients;

public interface IDeviceClient
{
    Task<bool> UnassignUserDevicesAsync(Guid userId);
}
