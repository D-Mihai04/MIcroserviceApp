using DS.UserService.Dtos;

namespace DS.UserService.services;

public interface IUserServices
{
    Task<List<UserDto>> GetAllAsync();

    Task<UserDto?> GetByIdAsync(Guid id);

    Task<UserDto?> CreateAsync(CreateUserDto request);

    Task<UserDto?> UpdateAsync(
        Guid id,
        UpdateUserDto request);

    Task<bool> DeleteAsync(Guid id);
}
