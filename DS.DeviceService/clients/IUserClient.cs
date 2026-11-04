namespace DS.DeviceService.clients;

public interface IUserClient
{
    Task<bool> UserExistsAsync(Guid userId);
}
