using System.Net.Http.Json;

namespace DS.AuthService.clients;

public class UserHttpClient : IUserClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserHttpClient> _logger;

    public UserHttpClient(HttpClient httpClient, ILogger<UserHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task CreateUserAsync(Guid id, string name, string email, int userType, string password)
    {
        try
        {
            var body = new
            {
                id,
                name = string.IsNullOrWhiteSpace(name) ? email : name,
                email,
                userType,
                password
            };

            var response = await _httpClient.PostAsJsonAsync("/users", body);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Failed to create user profile in UserService: {StatusCode} {Content}", response.StatusCode, content);
            }
            else
            {
                _logger.LogInformation("Successfully synced user {Email} ({Id}) to UserService.", email, id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while calling UserService to create user {Email}", email);
        }
    }
}
