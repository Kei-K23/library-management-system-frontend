namespace LibraryManagementSystemApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpContextService _httpContextService;

        public ApiService(HttpClient httpClient, HttpContextService httpContextService)
        {
            _httpClient = httpClient;
            _httpContextService = httpContextService;
        }

        public async Task<(TResponse Data, string ErrorMessage)> SendRequestAsync<TResponse>(HttpMethod method, string url, object? content = null)
        {
            string errorMessage = "";

            // Ensure HttpContextService is not null
            if (_httpContextService == null)
            {
                return (default, "HttpContextService is not available.");
            }
            // Fetch the token from the cookie
            var token = _httpContextService.GetCookie("authToken");
            if (string.IsNullOrEmpty(token))
            {
                return (default, "No token found.");
            }

            try
            {
                var request = new HttpRequestMessage(method, url);

                // Attach the token if available
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                // Add content if available and it's not a GET request
                if (content != null && method != HttpMethod.Get || method != HttpMethod.Delete)
                {
                    request.Content = JsonContent.Create(content);
                }

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<TResponse>();
                    return (data, null);
                }
                else
                {
                    errorMessage = await response.Content.ReadAsStringAsync();
                    return (default, $"Error: {response.StatusCode}, {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                return (default, $"Exception: {ex.Message}");
            }
        }
    }
}
