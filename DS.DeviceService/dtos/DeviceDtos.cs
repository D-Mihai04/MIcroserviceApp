namespace DS.DeviceService.Dtos;

public record DeviceDto(Guid Id, string DeviceName, int ConsumptionValue, Guid? UserId);

public record CreateDeviceDto(string DeviceName, int ConsumptionValue, Guid? UserId = null);

public record UpdateDeviceDto(
    string? DeviceName = null,
    int? ConsumptionValue = null,
    Guid? UserId = null
);
