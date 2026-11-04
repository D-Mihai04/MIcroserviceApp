using System.Security.Claims;
using DS.AuthService.entities;

namespace DS.AuthService.services;

public interface IJwtService
{
    (string Token, DateTime ExpiresAt) GenerateToken(Credential credential);
    ClaimsPrincipal? ValidateToken(string token);
}
