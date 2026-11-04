using System.Net;

namespace DS.DeviceService.clients;

public class UserHttpClient : IUserClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserHttpClient> _logger;

    public UserHttpClient(HttpClient httpClient, ILogger<UserHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> UserExistsAsync(Guid userId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/users/{userId}");
            request.Headers.Add("X-User-Role", "Admin");

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            _logger.LogWarning("Unexpected response status {StatusCode} when verifying user {UserId}", response.StatusCode, userId);
            return false;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to reach UserService when verifying user {UserId}", userId);
            throw new InvalidOperationException("User service is currently unavailable.", ex);
        }
    }
}
