using DS.UserService.Entities;
using Microsoft.EntityFrameworkCore;

namespace DS_Application.UserService.data;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}
