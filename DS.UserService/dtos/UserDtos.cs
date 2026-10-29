namespace DS.UserService.Dtos;

public record UserDto(Guid Id, string Name, string Email, int UserType);

public record CreateUserDto(string? Name, int UserType, string? Email, string? Password);

public record UpdateUserDto(
    string? Name = null,
    int? UserType = null
);
