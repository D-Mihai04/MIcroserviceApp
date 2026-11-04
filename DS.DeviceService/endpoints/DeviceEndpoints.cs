using DS.DeviceService.Dtos;
using DS.DeviceService.services;

namespace DS.DeviceService.endpoints;

public static class DeviceEndpoints
{
    public static void MapDeviceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/devices")
            .WithTags("Devices");

        // GET all devices (Admin only)
        group.MapGet("/", async (IDeviceServices deviceServices) =>
        {
            var devices = await deviceServices.GetAllAsync();
            return Results.Ok(devices);
        })
        .WithName("GetAllDevices")
        .WithSummary("Retrieve all metering devices (Admin only)")
        .Produces<List<DeviceDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .AddEndpointFilter<RequireAdminRoleFilter>();

        // GET device by ID (Admin only)
        group.MapGet("/{id:guid}", async (IDeviceServices deviceServices, Guid id) =>
        {
            var device = await deviceServices.GetByIdAsync(id);
            if (device is null)
            {
                return Results.NotFound($"Device with ID {id} was not found.");
            }
            return Results.Ok(device);
        })
        .WithName("GetDeviceById")
        .WithSummary("Retrieve a device by its unique ID (Admin only)")
        .Produces<DeviceDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .AddEndpointFilter<RequireAdminRoleFilter>();

        // GET all devices for a user (Client can only view their own devices; Admin can view any user's devices)
        group.MapGet("/user/{userId:guid}", async (IDeviceServices deviceServices, Guid userId, HttpContext context) =>
        {
            var role = context.Request.Headers["X-User-Role"].ToString();
            var callerId = context.Request.Headers["X-User-Id"].ToString();

            if (role != "Admin" && !string.Equals(callerId, userId.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Results.Json(new { error = "Forbidden: Clients may only view their own devices." }, statusCode: StatusCodes.Status403Forbidden);
            }

            var devices = await deviceServices.GetByUserIdAsync(userId);
            return Results.Ok(devices);
        })
        .WithName("GetDevicesByUserId")
        .WithSummary("Retrieve all devices assigned to a specific user (Admin or assigned Client)")
        .Produces<List<DeviceDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden);

        // POST create new device (Admin only)
        group.MapPost("/", async (IDeviceServices deviceServices, CreateDeviceDto request) =>
        {
            if (string.IsNullOrWhiteSpace(request.DeviceName))
            {
                return Results.BadRequest("DeviceName is required.");
            }

            try
            {
                var createdDevice = await deviceServices.CreateAsync(request);
                return Results.Created($"/devices/{createdDevice.Id}", createdDevice);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CreateDevice")
        .WithSummary("Create a new metering device (Admin only)")
        .Produces<DeviceDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden)
        .AddEndpointFilter<RequireAdminRoleFilter>();

        // PATCH update device details (Admin only)
        group.MapPatch("/{id:guid}", async (IDeviceServices deviceServices, Guid id, UpdateDeviceDto request) =>
        {
            try
            {
                var updatedDevice = await deviceServices.UpdateAsync(id, request);
                if (updatedDevice is null)
                {
                    return Results.NotFound($"Device with ID {id} was not found.");
                }
                return Results.Ok(updatedDevice);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("UpdateDevice")
        .WithSummary("Update device name or maximum hourly consumption (Admin only)")
        .Produces<DeviceDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .AddEndpointFilter<RequireAdminRoleFilter>();

        // PATCH assign device to a user (Admin only)
        group.MapPatch("/{id:guid}/assign/{userId:guid}", async (IDeviceServices deviceServices, Guid id, Guid userId) =>
        {
            try
            {
                var assignedDevice = await deviceServices.AssignUserAsync(id, userId);
                if (assignedDevice is null)
                {
                    return Results.NotFound($"Device with ID {id} was not found.");
                }
                return Results.Ok(assignedDevice);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("AssignDeviceToUser")
        .WithSummary("Assign a metering device to a user (Admin only)")
        .Produces<DeviceDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .AddEndpointFilter<RequireAdminRoleFilter>();

        // PATCH unassign device from any user (Admin only)
        group.MapPatch("/{id:guid}/unassign", async (IDeviceServices deviceServices, Guid id) =>
        {
            var unassignedDevice = await deviceServices.UnassignUserAsync(id);
            if (unassignedDevice is null)
            {
                return Results.NotFound($"Device with ID {id} was not found.");
            }
            return Results.Ok(unassignedDevice);
        })
        .WithName("UnassignDevice")
        .WithSummary("Remove user assignment from a device (Admin only)")
        .Produces<DeviceDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .AddEndpointFilter<RequireAdminRoleFilter>();

        // PATCH unassign all devices for a user (Admin only)
        group.MapPatch("/user/{userId:guid}/unassign", async (IDeviceServices deviceServices, Guid userId) =>
        {
            await deviceServices.UnassignAllByUserIdAsync(userId);
            return Results.Ok(new { message = $"All devices unassigned for user {userId}." });
        })
        .WithName("UnassignAllUserDevices")
        .WithSummary("Unassign all devices linked to a deleted user (Admin only)")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .AddEndpointFilter<RequireAdminRoleFilter>();

        // DELETE device by ID (Admin only)
        group.MapDelete("/{id:guid}", async (IDeviceServices deviceServices, Guid id) =>
        {
            var deleted = await deviceServices.DeleteAsync(id);
            if (!deleted)
            {
                return Results.NotFound($"Device with ID {id} was not found.");
            }
            return Results.Ok();
        })
        .WithName("DeleteDevice")
        .WithSummary("Delete a metering device (Admin only)")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .AddEndpointFilter<RequireAdminRoleFilter>();
    }
}

public class RequireAdminRoleFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var role = context.HttpContext.Request.Headers["X-User-Role"].ToString();

        if (role == "Admin")
        {
            return await next(context);
        }

        return Results.Json(new { error = "Admin role required." }, statusCode: StatusCodes.Status403Forbidden);
    }
}
