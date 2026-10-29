

namespace DS.UserService.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public UserType Type { get; set; }

    public enum UserType
    {
        Admin=1,
        Client=2

    }
}