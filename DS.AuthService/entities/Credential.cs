namespace DS.AuthService.entities;

public class Credential
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Client;

    public Guid UserId { get; set; }
}
