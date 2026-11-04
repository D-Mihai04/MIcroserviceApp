using DS.AuthService.entities;
using Microsoft.EntityFrameworkCore;

namespace DS.AuthService.data;

public class CredentialRepository : ICredentialRepo
{
    private readonly CredentialContext _context;

    public CredentialRepository(CredentialContext context)
    {
        _context = context;
    }

    public async Task<Credential?> GetByUsernameAsync(string username)
    {
        return await _context.Credentials
            .FirstOrDefaultAsync(c => c.Username.ToLower() == username.ToLower());
    }

    public async Task<Credential?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Credentials
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Credential> CreateAsync(Credential credential)
    {
        _context.Credentials.Add(credential);
        await _context.SaveChangesAsync();
        return credential;
    }

    public async Task<bool> UpdateAsync(Credential credential)
    {
        _context.Credentials.Update(credential);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(string username)
    {
        return await _context.Credentials
            .AnyAsync(c => c.Username.ToLower() == username.ToLower());
    }

    public async Task<bool> DeleteByUserIdAsync(Guid userId)
    {
        var credential = await GetByUserIdAsync(userId);
        if (credential is null)
            return false;

        _context.Credentials.Remove(credential);
        await _context.SaveChangesAsync();
        return true;
    }
}
