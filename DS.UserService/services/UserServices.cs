using DS.UserService.Entities;
using DS.UserService.Dtos;
using DS_Application.UserService.data;

namespace DS.UserService.services;

public sealed class UserServices : IUserServices
{
        private readonly IUserRepo _userRepository;

    public UserServices(IUserRepo userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users
            .Select(ToDto)
            .ToList();
    }
    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user is null ? null : ToDto(user);
    }

    public async Task<UserDto?> CreateAsync(CreateUserDto request)
    {
        if (!Enum.IsDefined(typeof(User.UserType), request.UserType))
            throw new ArgumentException($"Invalid user type value: {request.UserType}", nameof(request.UserType));

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name ?? string.Empty,
            Email = request.Email ?? string.Empty,
            Password = request.Password ?? string.Empty,
            Type = (User.UserType)request.UserType      
        };
        await _userRepository.CreateAsync(user);
        return ToDto(user);
    }

    public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            return null;

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            user.Name = request.Name;
        }

        if (request.UserType.HasValue)
        {
            if (!Enum.IsDefined(typeof(User.UserType), request.UserType.Value))
                throw new ArgumentException($"Invalid user type value: {request.UserType.Value}", nameof(request.UserType));

            user.Type = (User.UserType)request.UserType.Value;
        }

        await _userRepository.UpdateAsync(user);
        return ToDto(user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            return await _userRepository.DeleteAsync(id);
        }
        catch (Exception)
        {
            return false;
        }
    }


    private static UserDto ToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.Name,
            user.Email,
            (int)user.Type);
    }


}