using DS_Application.DeviceService.data;
using DS_Application.DeviceService.entities;
using DS.DeviceService.clients;
using DS.DeviceService.Dtos;

namespace DS.DeviceService.services;

public sealed class DeviceServices : IDeviceServices
{
    private readonly IDeviceRepo _deviceRepository;
    private readonly IUserClient _userClient;

    public DeviceServices(IDeviceRepo deviceRepository, IUserClient userClient)
    {
        _deviceRepository = deviceRepository;
        _userClient = userClient;
    }

    public async Task<List<DeviceDto>> GetAllAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();
        return devices.Select(ToDto).ToList();
    }

    public async Task<DeviceDto?> GetByIdAsync(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        return device is null ? null : ToDto(device);
    }

    public async Task<DeviceDto> CreateAsync(CreateDeviceDto request)
    {
        if (request.UserId.HasValue)
        {
            var userExists = await _userClient.UserExistsAsync(request.UserId.Value);
            if (!userExists)
            {
                throw new ArgumentException($"User with ID {request.UserId.Value} does not exist.");
            }
        }

        var device = new Device
        {
            Id = Guid.NewGuid(),
            DeviceName = request.DeviceName,
            ConsumptionValue = request.ConsumptionValue,
            UserId = request.UserId
        };

        await _deviceRepository.CreateAsync(device);
        return ToDto(device);
    }

    public async Task<DeviceDto?> UpdateAsync(Guid id, UpdateDeviceDto request)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device is null)
            return null;

        if (!string.IsNullOrWhiteSpace(request.DeviceName))
        {
            device.DeviceName = request.DeviceName;
        }

        if (request.ConsumptionValue.HasValue)
        {
            device.ConsumptionValue = request.ConsumptionValue.Value;
        }

        if (request.UserId.HasValue)
        {
            var userExists = await _userClient.UserExistsAsync(request.UserId.Value);
            if (!userExists)
            {
                throw new ArgumentException($"User with ID {request.UserId.Value} does not exist.");
            }
            device.UserId = request.UserId.Value;
        }

        await _deviceRepository.UpdateAsync(device);
        return ToDto(device);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            return await _deviceRepository.DeleteAsync(id);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<DeviceDto>> GetByUserIdAsync(Guid userId)
    {
        var devices = await _deviceRepository.GetByUserIdAsync(userId);
        return devices.Select(ToDto).ToList();
    }

    public async Task<DeviceDto?> AssignUserAsync(Guid deviceId, Guid userId)
    {
        var userExists = await _userClient.UserExistsAsync(userId);
        if (!userExists)
        {
            throw new ArgumentException($"User with ID {userId} does not exist.");
        }

        var device = await _deviceRepository.GetByIdAsync(deviceId);
        if (device is null)
            return null;

        device.UserId = userId;
        await _deviceRepository.UpdateAsync(device);
        return ToDto(device);
    }

    public async Task<DeviceDto?> UnassignUserAsync(Guid deviceId)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId);
        if (device is null)
            return null;

        device.UserId = null;
        await _deviceRepository.UpdateAsync(device);
        return ToDto(device);
    }

    public async Task<int> UnassignAllByUserIdAsync(Guid userId)
    {
        return await _deviceRepository.UnassignUserDevicesAsync(userId);
    }

    private static DeviceDto ToDto(Device device)
    {
        return new DeviceDto(
            device.Id,
            device.DeviceName,
            device.ConsumptionValue,
            device.UserId
        );
    }
}
