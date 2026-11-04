using DS.AuthService.dtos;
using DS.AuthService.services;

namespace DS.AuthService.endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Authentication");

        // POST /auth/register
        group.MapPost("/register", async (IAuthServices authServices, RegisterRequestDto request) =>
        {
            try
            {
                var response = await authServices.RegisterAsync(request);
                return Results.Ok(response);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("Register")
        .WithSummary("Register a new user account (Admin or Client) and issue JWT")
        .Produces<AuthResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);

        // POST /auth/login
        group.MapPost("/login", async (IAuthServices authServices, LoginRequestDto request) =>
        {
            try
            {
                var response = await authServices.LoginAsync(request);
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Json(new { error = ex.Message }, statusCode: StatusCodes.Status401Unauthorized);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("Login")
        .WithSummary("Authenticate user with username and password, returning JWT")
        .Produces<AuthResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        // GET /auth/validate -- proxy
        group.MapGet("/validate", (IAuthServices authServices, HttpContext context) =>
        {
            var authHeader = context.Request.Headers.Authorization.ToString();
            var token = !string.IsNullOrWhiteSpace(authHeader)
                ? authHeader
                : context.Request.Query["token"].ToString();

            if (string.IsNullOrWhiteSpace(token))
            {
                return Results.Json(new { error = "No token provided." }, statusCode: StatusCodes.Status401Unauthorized);
            }

            var result = authServices.ValidateToken(token);
            if (!result.IsValid)
            {
                return Results.Json(new { error = "Invalid or expired token." }, statusCode: StatusCodes.Status401Unauthorized);
            }

            if (result.UserId.HasValue)
            {
                context.Response.Headers["X-User-Id"] = result.UserId.Value.ToString();
            }
            if (!string.IsNullOrWhiteSpace(result.Role))
            {
                context.Response.Headers["X-User-Role"] = result.Role;
            }
            if (!string.IsNullOrWhiteSpace(result.Username))
            {
                context.Response.Headers["X-User-Name"] = result.Username;
            }

            return Results.Ok(result);
        })
        .WithName("ValidateToken")
        .WithSummary("Validate JWT token for Traefik ForwardAuth gateway middleware")
        .Produces<TokenValidationDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // PATCH /auth/user/{userId}
        group.MapPatch("/user/{userId:guid}", async (IAuthServices authServices, Guid userId, UpdateCredentialDto request) =>
        {
            try
            {
                var updated = await authServices.UpdateCredentialAsync(userId, request);
                if (!updated)
                {
                    return Results.NotFound(new { error = $"Credential for user {userId} not found." });
                }
                return Results.Ok(new { message = $"Credential for user {userId} updated." });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("UpdateCredential")
        .WithSummary("Update username or role for an existing credential")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        // DELETE /auth/user/{userId}
        group.MapDelete("/user/{userId:guid}", async (IAuthServices authServices, Guid userId) =>
        {
            var deleted = await authServices.DeleteCredentialAsync(userId);
            if (!deleted)
            {
                return Results.NotFound(new { error = $"Credential for user {userId} not found." });
            }
            return Results.Ok(new { message = $"Credential for user {userId} deleted." });
        })
        .WithName("DeleteCredential")
        .WithSummary("Delete user credentials from the database")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
