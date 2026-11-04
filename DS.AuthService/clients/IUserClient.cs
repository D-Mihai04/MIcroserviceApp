namespace DS.AuthService.clients;

public interface IUserClient
{
    Task CreateUserAsync(Guid id, string name, string email, int userType, string password);
}
