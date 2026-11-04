namespace DS.UserService.Dtos;

public record UserDto(Guid Id, string Name, string Email, int UserType);

public record CreateUserDto(
    string? Name,
    int UserType,
    string? Email,
    string? Password,
    Guid? Id = null
);

public record UpdateUserDto(
    string? Name = null,
    int? UserType = null
);
