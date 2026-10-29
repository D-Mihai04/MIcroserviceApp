using DS.UserService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DS_Application.UserService.data;

public class UserRepository : IUserRepo
{
    private readonly UserContext _context;

    public UserRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Users
            .AnyAsync(user => user.Id == id);
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
