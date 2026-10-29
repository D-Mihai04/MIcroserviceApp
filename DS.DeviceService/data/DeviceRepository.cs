using DS_Application.DeviceService.entities;
using Microsoft.EntityFrameworkCore;

namespace DS_Application.DeviceService.data;

public class DeviceRepository : IDeviceRepo
{
    private readonly DeviceContext _context;

    public DeviceRepository(DeviceContext context)
    {
        _context = context;
    }

    public async Task<List<Device>> GetAllAsync()
    {
        return await _context.Devices.ToListAsync();
    }

    public async Task<Device?> GetByIdAsync(Guid id)
    {
        return await _context.Devices.FindAsync(id);
    }

    public async Task<Device> CreateAsync(Device device)
    {
        _context.Devices.Add(device);
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device is null)
            return false;

        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Device> UpdateAsync(Device device)
    {
        _context.Devices.Update(device);
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Devices.AnyAsync(d => d.Id == id);
    }

    public async Task<Device?> GetByDeviceNameAsync(string deviceName)
    {
        return await _context.Devices.FirstOrDefaultAsync(d => d.DeviceName == deviceName);
    }

    public async Task<List<Device>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Devices.Where(d => d.UserId == userId).ToListAsync();
    }

    public async Task<int> UnassignUserDevicesAsync(Guid userId)
    {
        var devices = await _context.Devices.Where(d => d.UserId == userId).ToListAsync();
        foreach (var device in devices)
        {
            device.UserId = null;
        }
        return await _context.SaveChangesAsync();
    }
}
