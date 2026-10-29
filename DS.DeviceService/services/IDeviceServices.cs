using DS.DeviceService.Dtos;

namespace DS.DeviceService.services;

public interface IDeviceServices
{
    Task<List<DeviceDto>> GetAllAsync();
    Task<DeviceDto?> GetByIdAsync(Guid id);
    Task<DeviceDto> CreateAsync(CreateDeviceDto request);
    Task<DeviceDto?> UpdateAsync(Guid id, UpdateDeviceDto request);
    Task<bool> DeleteAsync(Guid id);
    Task<List<DeviceDto>> GetByUserIdAsync(Guid userId);
    Task<DeviceDto?> AssignUserAsync(Guid deviceId, Guid userId);
    Task<DeviceDto?> UnassignUserAsync(Guid deviceId);
    Task<int> UnassignAllByUserIdAsync(Guid userId);
}
