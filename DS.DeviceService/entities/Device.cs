namespace DS_Application.DeviceService.entities;


public class Device
{
    public Guid Id { get; set; }

    public string DeviceName { get; set; } = string.Empty;

    public int ConsumptionValue { get; set; }

    public Guid? UserId { get; set; }
}
