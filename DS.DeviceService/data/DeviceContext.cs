using DS_Application.DeviceService.entities;
using Microsoft.EntityFrameworkCore;

namespace DS_Application.DeviceService.data;

public class DeviceContext : DbContext
{
    public DeviceContext(DbContextOptions<DeviceContext> options)
        : base(options)
    {
    }

    public DbSet<Device> Devices => Set<Device>();

}