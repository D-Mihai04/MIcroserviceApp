using DS_Application.DeviceService.entities;

namespace DS_Application.DeviceService.data;

public interface IDeviceRepo
{
    Task<List<Device>> GetAllAsync();

    Task<Device?> GetByIdAsync(Guid id);

    Task<Device> CreateAsync(Device device);

    Task<bool> DeleteAsync(Guid id);

    Task<Device> UpdateAsync(Device device);

    Task<bool> ExistsAsync(Guid id);

    Task<Device?> GetByDeviceNameAsync(string deviceName);

    Task<List<Device>> GetByUserIdAsync(Guid userId);

    Task<int> UnassignUserDevicesAsync(Guid userId);
}
