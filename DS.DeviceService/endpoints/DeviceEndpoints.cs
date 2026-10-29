using DS.DeviceService.Dtos;
using DS.DeviceService.services;

namespace DS.DeviceService.endpoints;

public static class DeviceEndpoints
{
    public static void MapDeviceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/devices");

        // GET all devices
        group.MapGet("/", async (
            IDeviceServices deviceServices) =>
        {
            var devices = await deviceServices.GetAllAsync();
            return Results.Ok(devices);
        });

        // GET device by ID
        group.MapGet("/{id:guid}", async (
            IDeviceServices deviceServices, Guid id) =>
        {
            var device = await deviceServices.GetByIdAsync(id);
            if (device is null)
            {
                return Results.NotFound($"Device with ID {id} was not found.");
            }
            return Results.Ok(device);
        });

        // GET all devices for a specific user
        group.MapGet("/user/{userId:guid}", async (
            IDeviceServices deviceServices, Guid userId) =>
        {
            var devices = await deviceServices.GetByUserIdAsync(userId);
            return Results.Ok(devices);
        });

        // POST create new device
        group.MapPost("/", async (
            IDeviceServices deviceServices, CreateDeviceDto request) =>
        {
            if (string.IsNullOrWhiteSpace(request.DeviceName))
            {
                return Results.BadRequest("DeviceName is required.");
            }

            var createdDevice = await deviceServices.CreateAsync(request);
            return Results.Created($"/devices/{createdDevice.Id}", createdDevice);
        });

        // PATCH update device
        group.MapPatch("/{id:guid}", async (
            IDeviceServices deviceServices, UpdateDeviceDto request, Guid id) =>
        {
            var updatedDevice = await deviceServices.UpdateAsync(id, request);
            if (updatedDevice is null)
            {
                return Results.NotFound($"Device with ID {id} was not found.");
            }
            return Results.Ok(updatedDevice);
        });

        // PATCH assign device to a user
        group.MapPatch("/{id:guid}/assign/{userId:guid}", async (
            IDeviceServices deviceServices, Guid id, Guid userId) =>
        {
            var updatedDevice = await deviceServices.AssignUserAsync(id, userId);
            if (updatedDevice is null)
            {
                return Results.NotFound($"Device with ID {id} was not found.");
            }
            return Results.Ok(updatedDevice);
        });

        // PATCH unassign device from user
        group.MapPatch("/{id:guid}/unassign", async (
            IDeviceServices deviceServices, Guid id) =>
        {
            var updatedDevice = await deviceServices.UnassignUserAsync(id);
            if (updatedDevice is null)
            {
                return Results.NotFound($"Device with ID {id} was not found.");
            }
            return Results.Ok(updatedDevice);
        });

        // DELETE device by ID
        group.MapDelete("/{id:guid}", async (
            IDeviceServices deviceServices, Guid id) =>
        {
            var deleted = await deviceServices.DeleteAsync(id);
            if (!deleted)
            {
                return Results.NotFound($"Device with ID {id} was not found.");
            }
            return Results.Ok();
        });

        // DELETE /devices/user/{userId} (Unassign devices when a user is deleted in UserService)
        group.MapDelete("/user/{userId:guid}", async (
            IDeviceServices deviceServices, Guid userId) =>
        {
            var affected = await deviceServices.UnassignAllByUserIdAsync(userId);
            return Results.Ok(new { message = $"Unassigned {affected} devices for user {userId}." });
        });
    }
}
