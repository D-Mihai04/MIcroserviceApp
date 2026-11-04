namespace DS.UserService.clients;

public interface IAuthClient
{
    Task RegisterCredentialAsync(string username, string password, int role, Guid userId);
    Task UpdateCredentialAsync(Guid userId, string? username, int? role);
    Task DeleteCredentialAsync(Guid userId);
}
