namespace DS.UserService.clients;

public class DeviceHttpClient : IDeviceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DeviceHttpClient> _logger;

    public DeviceHttpClient(HttpClient httpClient, ILogger<DeviceHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> UnassignUserDevicesAsync(Guid userId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, $"/devices/user/{userId}/unassign");
            request.Headers.Add("X-User-Role", "Admin");

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogWarning("DeviceService returned status code {StatusCode} when unassigning devices for user {UserId}", response.StatusCode, userId);
            return false;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to reach DeviceService to unassign devices for user {UserId}", userId);
            return false;
        }
    }
}
