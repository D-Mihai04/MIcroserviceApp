using DS.AuthService.dtos;

namespace DS.AuthService.services;

public interface IAuthServices
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    TokenValidationDto ValidateToken(string token);
    Task<bool> UpdateCredentialAsync(Guid userId, UpdateCredentialDto request);
    Task<bool> DeleteCredentialAsync(Guid userId);
}
