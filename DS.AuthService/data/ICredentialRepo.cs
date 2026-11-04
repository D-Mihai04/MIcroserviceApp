using DS.AuthService.entities;

namespace DS.AuthService.data;

public interface ICredentialRepo
{
    Task<Credential?> GetByUsernameAsync(string username);
    Task<Credential?> GetByUserIdAsync(Guid userId);
    Task<Credential> CreateAsync(Credential credential);
    Task<bool> UpdateAsync(Credential credential);
    Task<bool> ExistsAsync(string username);
    Task<bool> DeleteByUserIdAsync(Guid userId);
}
