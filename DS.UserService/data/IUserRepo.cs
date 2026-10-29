using DS.UserService.Entities;

namespace DS_Application.UserService.data;

    public interface IUserRepo
{
    Task<List<User>> GetAllAsync();

    Task<User?> GetByIdAsync(Guid id);

    Task<User> CreateAsync(User user);

    Task<bool> DeleteAsync(Guid id);

    Task<User> UpdateAsync(User user);

    Task<bool> ExistsAsync(Guid id);

    Task<User?> GetByEmailAsync(string email);
}