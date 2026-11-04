namespace DS.AuthService.dtos;

public record RegisterRequestDto(
    string Username,
    string Password,
    int Role = 2,
    Guid? UserId = null,
    string? Name = null
);

public record LoginRequestDto(
    string Username,
    string Password
);

public record AuthResponseDto(
    string Token,
    Guid UserId,
    string Username,
    string Role,
    DateTime ExpiresAt
);

public record TokenValidationDto(
    bool IsValid,
    Guid? UserId = null,
    string? Username = null,
    string? Role = null
);

public record UpdateCredentialDto(
    string? Username = null,
    int? Role = null
);
