using DS.UserService.services;
using DS.UserService.Dtos;
namespace DS.UserService.api;

public static class UserEndpoints
{
    public static void MapUsersEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users");
          // .RequireAuthorization("AdminOnly");

        group.MapGet("/", async (
        IUserServices userServices) =>
        {
            var users = await userServices.GetAllAsync();

            return Results.Ok(users);
        });

        group.MapGet("/{id}", async (
            IUserServices userServices,
            Guid id) =>
        {
            var user = await userServices.GetByIdAsync(id);

            if (user == null)
            {
                return Results.NotFound(
                    $"User with ID {id} was not found.");
            }

            return Results.Ok(user);
        });

        group.MapPost("/", async (
            IUserServices userServices,
            CreateUserDto request) =>
        {
            try
            {
                var createdUser =
                    await userServices.CreateAsync(request);

                if (createdUser == null)
                {
                    return Results.BadRequest(
                        "Invalid user data.");
                }

                return Results.Ok(createdUser);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapPatch("/{id}", async (
            IUserServices userServices,
            UpdateUserDto request,
            Guid id) =>
        {
            try
            {
                var updatedUser =
                    await userServices.UpdateAsync(id, request);

                if (updatedUser == null)
                {
                    return Results.NotFound(
                        $"User with ID {id} was not found.");
                }

                return Results.Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });

        group.MapDelete("/{id}", async (
            IUserServices userServices,
            Guid id) =>
        {
            var deleted =
                await userServices.DeleteAsync(id);

            if (!deleted)
            {
                return Results.NotFound(
                    $"User with ID {id} was not found.");
            }

            return Results.Ok();
        });
    }
}