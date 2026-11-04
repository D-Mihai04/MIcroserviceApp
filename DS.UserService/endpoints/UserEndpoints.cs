using DS.UserService.services;
using DS.UserService.Dtos;

namespace DS.UserService.api;

public static class UserEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users")
            .WithTags("Users");

        group.MapGet("/", async (IUserServices userServices) =>
        {
            var users = await userServices.GetAllAsync();
            return Results.Ok(users);
        })
        .WithName("GetAllUsers")
        .WithSummary("Retrieve all registered users (Admin only)")
        .Produces<List<UserDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .AddEndpointFilter<RequireAdminRoleFilter>();

        group.MapGet("/{id}", async (IUserServices userServices, Guid id, HttpContext context) =>
        {
            var role = context.Request.Headers["X-User-Role"].ToString();
            var userIdHeader = context.Request.Headers["X-User-Id"].ToString();

            // Allow if Admin OR the client fetching their own profile
            if (role != "Admin" && !string.Equals(userIdHeader, id.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Results.Json(new { error = "Forbidden: Administrator role or matching user ID required." }, statusCode: StatusCodes.Status403Forbidden);
            }

            var user = await userServices.GetByIdAsync(id);
            if (user == null)
            {
                return Results.NotFound($"User with ID {id} was not found.");
            }
            return Results.Ok(user);
        })
        .WithName("GetUserById")
        .WithSummary("Retrieve a user profile by ID")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (IUserServices userServices, CreateUserDto request) =>
        {
            try
            {
                var createdUser = await userServices.CreateAsync(request);
                if (createdUser == null)
                {
                    return Results.BadRequest("Invalid user data.");
                }
                return Results.Ok(createdUser);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CreateUser")
        .WithSummary("Create a new user profile (Admin only)")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden)
        .AddEndpointFilter<RequireAdminRoleFilter>();

        group.MapPatch("/{id}", async (IUserServices userServices, UpdateUserDto request, Guid id) =>
        {
            try
            {
                var updatedUser = await userServices.UpdateAsync(id, request);
                if (updatedUser == null)
                {
                    return Results.NotFound($"User with ID {id} was not found.");
                }
                return Results.Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("UpdateUser")
        .WithSummary("Update user name or role (Admin only)")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound)
        .AddEndpointFilter<RequireAdminRoleFilter>();

        group.MapDelete("/{id}", async (IUserServices userServices, Guid id) =>
        {
            var deleted = await userServices.DeleteAsync(id);
            if (!deleted)
            {
                return Results.NotFound($"User with ID {id} was not found.");
            }
            return Results.Ok();
        })
        .WithName("DeleteUser")
        .WithSummary("Delete a user account and unassign their devices (Admin only)")
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

        return Results.Json(new { error = "Admin role required" }, statusCode: StatusCodes.Status403Forbidden);
    }
}
