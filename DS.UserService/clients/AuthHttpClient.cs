using System.Net.Http.Json;

namespace DS.UserService.clients;

public class AuthHttpClient : IAuthClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthHttpClient> _logger;

    public AuthHttpClient(HttpClient httpClient, ILogger<AuthHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task RegisterCredentialAsync(string username, string password, int role, Guid userId)
    {
        try
        {
            var body = new
            {
                username,
                password,
                role,
                userId
            };

            var response = await _httpClient.PostAsJsonAsync("/auth/register", body);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Failed to create credential in AuthService: {StatusCode} {Content}", response.StatusCode, content);
            }
            else
            {
                _logger.LogInformation("Credential for {Username} registered in AuthService.", username);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling AuthService to create credential for {Username}", username);
        }
    }

    public async Task DeleteCredentialAsync(Guid userId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/auth/user/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Failed to delete credential for user {UserId} in AuthService: {StatusCode} {Content}", userId, response.StatusCode, content);
            }
            else
            {
                _logger.LogInformation("Credential for user {UserId} deleted from AuthService.", userId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling AuthService to delete credential for user {UserId}", userId);
        }
    }

    public async Task UpdateCredentialAsync(Guid userId, string? username, int? role)
    {
        try
        {
            var body = new { username, role };
            var response = await _httpClient.PatchAsJsonAsync($"/auth/user/{userId}", body);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Failed to update credential in AuthService for user {UserId}: {StatusCode} {Content}", userId, response.StatusCode, content);
            }
            else
            {
                _logger.LogInformation("Credential for user {UserId} updated in AuthService.", userId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling AuthService to update credential for user {UserId}", userId);
        }
    }
}
