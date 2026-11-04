using System.Security.Claims;
using DS.AuthService.clients;
using DS.AuthService.data;
using DS.AuthService.dtos;
using DS.AuthService.entities;

namespace DS.AuthService.services;

public class AuthServices : IAuthServices
{
    private readonly ICredentialRepo _credentialRepo;
    private readonly IJwtService _jwtService;
    private readonly IUserClient _userClient;
    private readonly ILogger<AuthServices> _logger;

    public AuthServices(ICredentialRepo credentialRepo, IJwtService jwtService, IUserClient userClient, ILogger<AuthServices> logger)
    {
        _credentialRepo = credentialRepo;
        _jwtService = jwtService;
        _userClient = userClient;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            throw new ArgumentException("Username cannot be empty.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password cannot be empty.");

        if (await _credentialRepo.ExistsAsync(request.Username))
            throw new ArgumentException($"Username '{request.Username}' is already taken.");

        var role = Enum.IsDefined(typeof(UserRole), request.Role)
            ? (UserRole)request.Role
            : UserRole.Client;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var userId = (request.UserId.HasValue && request.UserId.Value != Guid.Empty)
            ? request.UserId.Value
            : Guid.NewGuid();

        var credential = new Credential
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Username = request.Username.Trim(),
            PasswordHash = passwordHash,
            Role = role
        };

        await _credentialRepo.CreateAsync(credential);

        var displayName = !string.IsNullOrWhiteSpace(request.Name) ? request.Name.Trim() : credential.Username;
        await _userClient.CreateUserAsync(userId, displayName, credential.Username, (int)credential.Role, request.Password);

        var (token, expiresAt) = _jwtService.GenerateToken(credential);

        _logger.LogInformation("User {Username} registered successfully with role {Role}", credential.Username, credential.Role);

        return new AuthResponseDto(
            token,
            credential.UserId,
            credential.Username,
            credential.Role.ToString(),
            expiresAt
        );
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            throw new UnauthorizedAccessException("Invalid username or password.");

        var credential = await _credentialRepo.GetByUsernameAsync(request.Username.Trim());
        if (credential is null)
            throw new UnauthorizedAccessException("Invalid username or password.");

        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, credential.PasswordHash);
        if (!passwordValid)
            throw new UnauthorizedAccessException("Invalid username or password.");

        var (token, expiresAt) = _jwtService.GenerateToken(credential);

        _logger.LogInformation("User {Username} logged in successfully", credential.Username);

        return new AuthResponseDto(
            token,
            credential.UserId,
            credential.Username,
            credential.Role.ToString(),
            expiresAt
        );
    }

    public TokenValidationDto ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return new TokenValidationDto(false);

        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = token.Substring(7).Trim();

        var principal = _jwtService.ValidateToken(token);
        if (principal is null)
            return new TokenValidationDto(false);

        var userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? principal.FindFirst("sub")?.Value;
        var username = principal.FindFirst(ClaimTypes.Name)?.Value;
        var role = principal.FindFirst(ClaimTypes.Role)?.Value;

        Guid? userId = Guid.TryParse(userIdStr, out var id) ? id : null;

        return new TokenValidationDto(
            true,
            userId,
            username,
            role
        );
    }

    public async Task<bool> UpdateCredentialAsync(Guid userId, UpdateCredentialDto request)
    {
        var credential = await _credentialRepo.GetByUserIdAsync(userId);
        if (credential is null)
            return false;

        if (!string.IsNullOrWhiteSpace(request.Username))
        {
            var targetUsername = request.Username.Trim();
            var existing = await _credentialRepo.GetByUsernameAsync(targetUsername);
            if (existing != null && existing.UserId != userId)
            {
                throw new ArgumentException($"Username '{targetUsername}' is already taken by another account.");
            }
            credential.Username = targetUsername;
        }

        if (request.Role.HasValue && Enum.IsDefined(typeof(UserRole), request.Role.Value))
        {
            credential.Role = (UserRole)request.Role.Value;
        }

        await _credentialRepo.UpdateAsync(credential);
        _logger.LogInformation("Updated credentials for user {UserId}: Username={Username}, Role={Role}", userId, credential.Username, credential.Role);
        return true;
    }

    public async Task<bool> DeleteCredentialAsync(Guid userId)
    {
        return await _credentialRepo.DeleteByUserIdAsync(userId);
    }
}
